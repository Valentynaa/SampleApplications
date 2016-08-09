'use strict';

const postCode = {
	CA: /^([ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ][\s\-]?[0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]$)/g,
	US: /^\d{5}$|^\d{9}$/g,
	AU: /^\d{4}$/g
}
exports.postCode = postCode;
