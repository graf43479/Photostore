$.validator.methods.range = function (value, element, param) {
    var globalizedValue = value.replace(",", ".");
    return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
}

$.validator.methods.number = function (value, element) {
    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
}


$.validator.methods.date = function (value, element) {
    if (!value)
        return false;

    var splitVal = value.split(' ');

    var date = splitVal[0].split('.');

    var time = (splitVal.length == 2 ? splitVal[1] : '0:0:0').split(':');

    return this.optional(element) || !/Invalid|NaN/.test(new Date(date[2], date[1], date[0], time[0], time[1], time[2], 0));
};