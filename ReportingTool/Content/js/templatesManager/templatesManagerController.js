'use strict';

templatesManagerModule.controller("templatesManagerController",
    ['$scope', '$stateParams', '$state', 'TemplateFactory', function($scope, $stateParams, $state, TemplateFactory) {

        $scope.templates = {};
        TemplateFactory.GetAllTemplates().then(templatesSuccess, templatesFail);

        function templatesSuccess(response) {
            $scope.templates = response.data;
        };

        function templatesFail(response) {
            alert("Error: " + response.code + " " + response.statusText);
        };
}]);