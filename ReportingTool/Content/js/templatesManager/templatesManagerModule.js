'use strict';

var templatesManagerModule = angular.module("templatesManagerModule", ['ui.router']);

templatesManagerModule.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

    var templatesManager = {
        name: 'mainView.templatesManager',
        url: '/templatesManager',

        views: {
            '': {
                templateUrl: 'Content/templates/templatesManager/templatesManagerView.html',
                controller: 'templatesManagerController'
            },
            'templatesList@mainView.templatesManager': {
                templateUrl: 'Content/templates/templatesManager/templatesList.html'
            }
        }
    };

    var templateFieldsView = {
        name: 'mainView.templatesManager.templateFieldsView',
        url: '/viewFields/{templateId:int}',
        templateUrl: 'Content/templates/templatesManager/viewFields.html',
        controller: 'templatesFieldsManagerController'
    };

    $stateProvider.state(templatesManager);
    $stateProvider.state(templateFieldsView);
}]);