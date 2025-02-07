$(document).ready(function () {
    // First, define the validation method
    $.validator.addMethod("customdate", function (value, element) {
        if (!value) {
            return false; // Prevent empty values from passing validation
        }

        let selectedDate = new Date(value);
        let currentDate = new Date();

        // Reset time to midnight to compare only the date part
        selectedDate.setHours(0, 0, 0, 0);
        currentDate.setHours(0, 0, 0, 0);

        return selectedDate >= currentDate;
    }, "Please select a date today or in the future.");

    // Then, register it with jQuery Unobtrusive Validation
    $.validator.unobtrusive.adapters.add("customdate", [], function (options) {
        options.rules["customdate"] = true;
        options.messages["customdate"] = options.message;
    });
});