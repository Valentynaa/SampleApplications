using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect.Models.Magento.Products;
using MagentoConnect.Utilities;
using MySql.Data.MySqlClient;

namespace MagentoConnect.Database
{
	/// <summary>
	/// Singleton for database connection to Magento Database
	/// Conection pool not used because only one user will be using application at a time
	/// </summary>
	public class DatabaseConnection
	{
		public const int DefaultPort = 3306;
		private const string ProductMediaDirectoryRoot = "catalog/product";
		private const string MediaStorageSettingPath = "system/media_storage_configuration/media_storage";

		private MySqlConnection _connection;

		/// <summary>
		/// Only object for the database connection
		/// </summary>
		private static DatabaseConnection _instance;
		public static DatabaseConnection Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new DatabaseConnection();
				}
				return _instance;
			}
		}

		/// <summary>
		/// Creates a database connection based on the values in App.config
		/// </summary>
		private DatabaseConnection()
		{
			ConfigReader.MagentoDatabasePort = GetValidPort(ConfigReader.MagentoDatabasePort);

			string connectionString = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};", ConfigReader.MagentoDatabaseServer, 
				ConfigReader.MagentoDatabasePort, ConfigReader.MagentoDatabaseName, ConfigReader.MagentoDatabaseUserId, ConfigReader.MagentoDatabasePassword);
			Initialize(connectionString);
		}

		/// <summary>
		/// Creates a database connection based on the database information passed in.
		/// </summary>
		/// <param name="server">Server for database</param>
		/// <param name="database">Database name</param>
		/// <param name="userId">User ID for database</param>
		/// <param name="password">Password for user</param>
		/// <param name="port">(optional) port for database connection</param>
		private DatabaseConnection(string server, string database, string userId, string password, int port = DefaultPort)
		{
			port = GetValidPort(port);
			
			string connectionString = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};", server, port, database, userId, password);
			Initialize(connectionString);
		}

		/// <summary>
		/// Initializes connection
		/// </summary>
		/// <param name="connectionString">String for connection information</param>
		private void Initialize(string connectionString)
		{
			_connection = new MySqlConnection(connectionString);
		}

		/// <summary>
		/// Opens the database connection
		/// Logs error if an error occurs
		/// </summary>
		/// <returns>If the connection opened successfully</returns>
		private bool OpenConnection()
		{
			try
			{
				_connection.Open();
				return true;
			}
			catch (MySqlException ex)
			{
				//The two most common error numbers when connecting are:
				//0: Cannot connect to server.
				//1045: Invalid user name and/or password.
				switch (ex.Number)
				{
					case 0:
						LogUtility.Write(Log.Error, "Cannot connect to database. Ensure database is configured correctly and connection string is correct.");
						break;

					case 1045:
						LogUtility.Write(Log.Error, "Cannot connect to database. Invalid username/password.");
						break;
				}
				return false;
			}
		}

		/// <summary>
		/// Closes the database connection
		/// Logs error if an error occurs
		/// </summary>
		/// <returns>If the connection closed successfully</returns>
		private bool CloseConnection()
		{
			try
			{
				_connection.Close();
				return true;
			}
			catch (MySqlException ex)
			{
				LogUtility.Write(Log.Error, string.Format("Error closing database connection: {0}", ex.Message));
				return false;
			}
		}

		/// <summary>
		/// Gets the bytes for the media entry
		/// </summary>
		/// <param name="mediaEntry">Entry to get bytes for</param>
		/// <returns>Byte data for medie entry</returns>
		public byte[] GetMediaGalleryEntryFile(MediaGalleryEntryResource mediaEntry)
		{
			if (mediaEntry == null)
				throw new ArgumentNullException(nameof(mediaEntry));

			string mediaFile = mediaEntry.file;

			string mediaDirectory = mediaFile.Substring(0, mediaFile.LastIndexOf("/", StringComparison.Ordinal));
			string fileName = mediaFile.Substring(mediaFile.LastIndexOf("/", StringComparison.Ordinal) + 1);
			string query = "SELECT * FROM magento.media_storage_file_storage WHERE directory = @directory AND filename = @filename;";

			//Open connection
			if (OpenConnection())
			{
				MySqlCommand command = new MySqlCommand(query, _connection);
				command.Parameters.AddWithValue("@directory", ProductMediaDirectoryRoot + mediaDirectory);
				command.Parameters.AddWithValue("@filename", fileName);

				MySqlDataAdapter adapter = new MySqlDataAdapter(command);

				DataSet dataSet = new DataSet();

				adapter.Fill(dataSet, "media_storage_file_storage");
				DataTable dataTable = dataSet.Tables["media_storage_file_storage"];

				//close Connection
				CloseConnection();

				//return media file bytes
				return (byte[]) dataTable.Rows[0]["content"];
			}
			return null;
		}

		/// <summary>
		/// Finds the media storage configuration setting in the database and returns the result
		/// </summary>
		/// <returns>Media storage setting in database</returns>
		public MediaStorageConfiguration GetMediaStorageConfiguration()
		{
			string query =
				"SELECT value FROM magento.core_config_data WHERE path = @mediaStorageSettingPath;";

			//Open connection
			if (OpenConnection())
			{
				MySqlCommand command = new MySqlCommand(query, _connection);
				command.Parameters.AddWithValue("@mediaStorageSettingPath", MediaStorageSettingPath);


				var result = command.ExecuteScalar().ToString();

				//close Connection
				CloseConnection();

				//value is 0 or 1 which gives the correct enumeration
				return (MediaStorageConfiguration)int.Parse(result);
			}
			return MediaStorageConfiguration.FileSystem;
		}

		/// <summary>
		/// Ensures that value is between the min and max port values:
		/// 1 => 2^16 - 1
		/// 
		/// If the port is invalid the default of 3306 is used.
		/// </summary>
		/// <param name="port">value to check</param>
		/// <returns>Valid port number</returns>
		private int GetValidPort(int port)
		{
			if (port < 1 || port > 65535)
			{
				Console.WriteLine("Database port {0} specified is not a valid port. Port {1} used instead.", port, DefaultPort);
				return DefaultPort;
			}
			return port;
		}
	}
}
