'use strict';

var templatesManagerModule = angular.module("templatesManagerModule", ['ui.router']);

templatesManagerModule.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

    var templatesManager = {
        name: 'mainView.templatesManager',
        url: '/templatesManager',

        views: {
            '': {
                templateUrl: 'Content/templates/templatesManager/templatesManagerView.html'
            },
            'templatesList@mainView.templatesManager': {
                templateUrl: 'Content/templates/templatesManager/templatesList.html'
            }
        }
    };

   $stateProvider.state(templatesManager);
}]);