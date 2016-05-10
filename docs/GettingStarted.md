# Getting Started

This guide goes over everything needed to run [MagentoConnect](https://github.com/iQmetrix/MagentoConnect).

The audience for this guide is intended to be developers interested in extending MagentoConnect.

---

## Prerequisites

Before checking out this project, ensure you have the following installed on your computer:

* Visual Studio 2012 or newer (NOTE: this project may be incompatible with older versions of VS)
* [Nuget](https://www.nuget.org/) package management solution
* [RestSharp](http://restsharp.org/) REST and HTTP API client for .Net
* [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) Nuget package for working with JSON 
* Internet Information Services
* [Python 3.4.4](https://www.python.org/downloads)

You must also have a [Magento](https://magento.com/) server running, and have **admin** access or access credentials.

## Knowledge Assumptions

This guide assumes the following:

* **Windows** Operating System
* Familiarity with **C#**
* Familiarity with **Git** and **Github**

---

## Setting Up Environment 

These steps must be done in **sequential order**. 

### 1.1 Prepare Site

1. Create a [magento account](https://www.magentocommerce.com/products/customer/account) and log in, this is *required* 
2. [Download Magento](https://www.magentocommerce.com/download)
    * I recommend **Full Release with Sample Data**  
3. Extract the ZIP into a folder named **Magento** 
    * Note: this make take up to 30 minutes
4. Move the folder to `c:/inetpub/wwwroot`

### 1.2 Prepare Permissions

**IMPORTANT**: This section is here as an alternative to [Create the Magento file system owner](http://devdocs.magento.com/guides/v2.0/install-gde/prereq/apache-user.html). 

**DO NOT** do this for a production system, it is very dangerous 

1. In file explorer, navigate to `c:/inetpub/wwwroot`
2. Right click on Magento -> Properties
3. Click **security**
4. Click **Edit**
5. For **every** user in the top list, ensure every **Allow** check box is selected, where possible
6. Press apply, ok, ok 

### 1.3 Install PHP and URL Rewrite

1. Open Internet Information Services Manager (IIS)
2. From the main view in the center, double click on Web Platform Installer
    * If you do not see this, download and install [Web Platform Installer|https://www.microsoft.com/web/downloads/platform.aspx] 
3. From Web Platform Installer, search for PHP 5.6 - **NOT** IIS express
4. Press the Add button next to PHP 5.6.X
5. Search for rewrite
6. Press the Add button next to URL Rewrite 2.0 
7. Press Install at bottom and Accept
8. Keep IIS Manager open

### 1.4 MySQL

1. [Download MySQL](https://dev.mysql.com/downloads/installer/) 
2. Choose the non-web version
3. Select **No thanks, just start my download** at the bottom of the page. No need to create an account.
4. Install MySQL
    * Select Developer when given a choice
    * Make sure to select **workbench** if given the option
    * Ensure Python 3.4 is installed, not Python 3.5
    * Make sure to record the password you provide, if you assign one 
5. Configure MySQL
    * Open MySQL Workbench
    * Press the + button next to MySQL Connections
        * Name: Magento
        * Default Schema: leave blank for now 
    * Press Test Connection
    * Enter password entered this in **Step 4**
6. Create Magento Schema
    * Double-click on Magento connection
    * In the Query window, enter: `CREATE SCHEMA 'magento';`
    * Click execute (Lightning bolt icon)
    * Close the Magento connection
7. Add Magento Schema
    * Right-click Magento connection and click **Edit Connection...** 
        * **Default Schema:** Magento
    * Click **Test Connection**
    * Click **Close**

### 1.5 Set Up IIS

1. Create an IIS Site
    * Open **Internet Information Services Manager**, if not already open
    * Expand your local connection in left pane.
    * Right click on Sites folder and click **Add Website**
        * **Site Name**: Magento
        * **Application Pool**: DefaultAppPool
        * **Physical Path**: `C:/inetpub/wwwroot/Magento`
        * **Port**: 82 
            * This is to ensure it does not conflict with other programs
4. Click **OK** 
5. Configure IIS
    * Click on Magento in left pane
    * Double-click **PHP Manager** under the **IIS** section in the main view
    * If there is a **PHP version**, then continue. Otherwise,
        * Select **Register new PHP version** 
        * Enter the location of your `php-cgi.exe` file, most likely here:
        * `C:\Program Files (x86)\PHP\v5.6\php-cgi.exe`

### 2.1 Set Up Magento

1. Navigate to [http://localhost:82](http://localhost:82/) 
2. If you see a setup screen, continue, otherwise something went wrong
    * Begin Setup
    * For PHP Extensions, go to PHP Manager in IIS
        * Select **Enable or disable an extension**, and enable the missing extensions
    * For PHP Settings, go to PHP Manager in IIS
        * Select the `php.ini` link next to **Configuration file** 
        * Change any configuration settings as requested from Magento  
3. Add a Database
    * Database Server Password = Password from **MySQL** Step 4 above
4. Web Configuration
    * Change admin address to /admin for easier use (such as /magento) 
5. Customize Your Store
    * OPTIONAL - change timezone 
6. Create Admin Account
    * Create account and **RECORD VALUES**
    * Click **Install Now** 
7. **If you're having issues installing**, such as it hanging around 90% do the following:
    * Close the setup and reopen
    * Under one of the last steps (Customize I believe) uncheck all modules that include SampleData.
    * This will speed up the process
    * If that doesn't work, delete contents from `c:/inetpub/wwwroot/Magento`:
        * `var/cache`
        * `var/generation`
        * `app/etc/config.php`
        * `app/etc/env.php`


### 2.2 Configure IIS Rewrite

1. Open notepad in **ADMIN MODE** (right click -> open as administrator) 
2. Navigate to `c:/inetpub/wwwroot`
3. Open `web.config` and add the following directly above `</system.webServer>`

```
<rewrite> 
    <rules> 
        <rule name="Remove index.php rule" stopProcessing="true"> 
            <match url=".*" ignoreCase="false" /> 
            <conditions> 
                <add input="{URL}" pattern="^/(media|skin|js)/" ignoreCase="false" negate="true" /> 
                <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" /> 
                <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" /> 
            </conditions> 
            <action type="Rewrite" url="index.php" /> 
        </rule> 
    </rules> 
</rewrite>
```

### 2.3 Fix Missing CSS

Magento and Windows do not get along, so the site will not generate CSS properly

1. Open a command prompt
2. `cd c:/inetpub/wwwroot/magento`
3. `php bin/magento setup:static-content:deploy`
    * If this returns a what is php? error, go to step 4, else go to step 5
4. Add PHP to Env 
    * Start -> Search -> Edit the system environment variables -> Environment Variables
    * Advanced tab, select Path in top selector
    * Press Edit
    * To the end of the string in Variable value, add `C:\Program Files (x86)\PHP\v5.6\php.exe`
    * Value may differ depending on what you used for **Setup IIS*** Step 5 
    * Ensure the previous value in the string has a ; after it, this is how they are separated 
5. Close the command prompt and refresh your Magento session.
 
## 2.4 Configuring Magento

1. Create Custom Attribute
    * Log into Magento
    * Select Stores (sidebar) -> Attributes -> Product
    * Click **Add New Attribute** and expand **Advanced Attribute Properties**
        * Default Label: iQmetrix Product Identifier
        * Attribute code: iqmetrixproductidentifier 
        * If you use any other value, ensure AppConfig MappingCode is adjusted
    * Scope: Global 
    * Click *Save Attribute* 
2. Make manufacturer Required
    * Select Stores (sidebar) -> Attributes -> Product
    * In the Attribute Code field, type **manufacturer** and hit **Enter** to search
    * Click **Edit** or click the row for manufacturer
    * Change **Values Required** to **Yes** 
    * Ensure there is at least one Manufacturer under Manage Options
    * Click **Add Option** 
    * Enter the same manufacturer name under both **Default Store View** and **Admin** (e.g. sony) 
    * Click *Save Attribute* 
3. Make name required
    * In the Attribute Code field, type **name** and hit **Enter** to search
    * Click **Edit** or click the row for name
    * Change **Values Required** to **Yes** 
    * Click **Save Attribute**
4. Make category Required
    * Select Stores (sidebar) -> Attributes -> Product
    * In the Attribute Code field, type **category** and press Enter to search
    * Click **Edit** or click the row for **category_ids**
    * Change **Values Required** to **Yes** 
    * Click **Save Attribute**
5. Create an Integration Account
    * Select System (sidebar) then Extensions -> Integrations
    * Click **Add New Integration**
    * Click **Integration Info** link and enter a value (anything) for **Name** 
    * Click **API** link and change **Resource Access** to **All**  
    * Click **Save** 
    * In the new row in the table, click **Activate** 
    * Click **Allow** 
    * **IMPORTANT**: Write down the **Consumer Key**, **Consumer Secret**, **Access Token** and **Access Token Secret**  
    * Click *Done* 
6. Download Project 
    * [Download Project](https://github.com/iQmetrix/MagentoConnect)
    * Open with Visual Studio
    * Ensure project is able to build, you may need the following extensions and libraries:
        * [Nuget](https://www.nuget.org)
        * [Restsharp](http://restsharp.org)
        * [Newtonsoft](https://www.nuget.org/packages/Newtonsoft.Json)

---

## Mapping

Before the App can be run, you must first create a series of Mapping values in [App.config](https://github.com/iQmetrix/MagentoConnect/blob/master/App.config).

### FieldMapping

**Format**:

```
<add key="{MagentoAttributeCode}" value="{EndlessAisleFieldDefinition}"/>
```

For each **field** you wish to sync from Magento to Endless Aisle, there must be an entry listed here.

* `MagentoAttributeCode` is the attribute_code value of the property you wish to sync to EA. 
    * Endpoint: `GET {MagentoLocation}/rest/V1/products/attributes?searchCriteria`
* `EndlessAisleFieldDefinition` is the Id of a FieldDefinition of the appropriate property to sync 
    * Endpoint: `GET https://productlibrary{{UrlSuffix}}.iqmetrix.net/v1/FieldDefinitions`
    * API Reference: [Getting All Field Definitions](http://developers.iqmetrix.com/api/field-definitions/#getting-all-field-definitions)

**Example**:

```
<add key="name" value="1"/>
```

### ManufacturerMapping

```
<add key="{MagentoManufacturerOptionId}" value="{EndlessAisleManufacturerId}"/>
```

For each Manufacturer option in Magento, there must be an entry listed here.

* `MagentoManufacturerOptionId` is the option.value of the manufacturer
    * Endpoint: `GET {MagentoLocation}/rest/V1/products/attributes/manufacturer`
* `EndlessAisleManufacturerId` is the Id of a matching Manufacturer 
    * Endpoint: `GET https://entitymanager{{UrlSuffix}}.iqmetrix.net/v1/Manufacturers`
    * API Reference: [Getting All Manufacturers](http://developers.iqmetrix.com/api/entity-store/#getting-all-manufacturers)

**Example**:

```
<add key="213" value="9827"/>
```
### CategoryMapping

```
<add key="{MagentoCategoryId}" value="{EndlessAisleClassificationId}"/>
```

For each category in Magento, there must be an entry listed here.

* `MagentoCategoryId` is the id of the category
    * Endpoint: `GET {MagentoLocation}/rest/V1/categories`
* `EndlessAisleClassificationId` is the Id of a matching Classification (NOT Category)
    * Endpoint: `GET https://productlibrary{{UrlSuffix}}.iqmetrix.net/v1/ClassificationTrees({{ClassificationTreeId}})`
    * API Reference: [Getting a Classification Tree](http://developers.iqmetrix.com/api/classification-tree/#getting-a-classification-tree)

**Example**:

```
<add value="396" key="38"/>
```

### ColorMapping

**NOTE**: This section is not required, but if omitted Products will not have Color Definitions in EA.

```
<add key="{MagentoColorOptionId}" value="{EndlessAisleColorTagId}"/>
```

* `MagentoColorOptionId` is the option.value of the color
    * Endpoint: `GET {MagentoLocation}/rest/V1/products/attributes/color`
* `EndlessAisleColorTagId` is the Id of a matching ColorTag 
    * Endpoint: `GET https://productlibrary{{UrlSuffix}}.iqmetrix.net/v1/ColorTags`
    * API Reference: [Getting All Color Tags](http://developers.iqmetrix.com/api/product-structure/#getting-all-color-tags)

**Example**:

```
<add value="1" key="49"/>
```

---

## Adding a Product to Endless Aisle Using Magento Connect

This section explains the steps needed to use Magento and MagentoConnect to add a new product to EA.

### Steps

1. Create a Product in Magento 
    * Log into Magento
    * Select Products (sidebar) -> Inventory -> Catalog
    * Click the down arrow in the **Add Product** button and then **Configurable**, **Virutual** OR **Simple Product** from the drop down list
        * Name
        * SKU
        * Categories
        * Manufacturer
    * Click **Save**  
2. Run App
    * Open App using Visual Studio
    * Build and run the application 