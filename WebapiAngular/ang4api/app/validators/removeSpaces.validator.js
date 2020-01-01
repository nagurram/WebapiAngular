"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
function removeSpaces(control) {
    if (control && control.value && !control.value.replace(/\s/g, '').length) {
        console.log('in remove spaces function');
        console.log(control.value);
        control.setValue('');
    }
    return null;
}
exports.removeSpaces = removeSpaces;
//# sourceMappingURL=removeSpaces.validator.js.map