angular.module('configurationModule').directive('projectName', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attr, ctrl) {
            function customValidator(ngModelValue) {
                if (/[a-z]/.test(ngModelValue)) {
                    ctrl.$setValidity('uppercaseValidator', false);
                } else {
                    ctrl.$setValidity('uppercaseValidator', true);
                }
                if (/[^.\-A-Z0-9]/.test(ngModelValue)) {
                    ctrl.$setValidity('symbolsValidator', false);
                } else {
                    ctrl.$setValidity('symbolsValidator', true);
                }
               
                return ngModelValue;
            }
            ctrl.$parsers.push(customValidator);
        }
    };
});