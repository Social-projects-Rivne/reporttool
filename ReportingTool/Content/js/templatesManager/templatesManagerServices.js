'use strict';

templatesManagerModule.factory('TemplateFactory', ['$http', function($http) {
            var templateFactory = {
                GetAllTemplates: GetAllTemplates
            };

            function GetAllTemplates() {
                var templates = $http.get("Templates/GetAllTemplates");
                return templates;
            };

            return templateFactory;

}]);