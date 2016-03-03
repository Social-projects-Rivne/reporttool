﻿'use strict';

var templatesManagerModule = angular.module("templatesManagerModule", ['ui.router', 'ui.bootstrap']);

templatesManagerModule.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

    var templatesManager = {
        name: 'mainView.templatesManager',
        url: '/templatesManager',

        views: {
            '': {
                templateUrl: 'Content/templates/templatesManager/templatesManagerView.html',
                controller: 'templatesManagerController'
            }
        }
    };

    var templatesList = {
        name: 'mainView.templatesManager.templatesList',
        url: '/templatesList',
        templateUrl: 'Content/templates/templatesManager/templateDetails.html'
    };

    var templateDetailsView = {
        name: 'mainView.templatesManager.templatesList.templateDetailsView',
        url: '/templateDetails/{templateId:int}',
        templateUrl: 'Content/templates/templatesManager/templateDetails.html',
        controller: 'templatesFieldsManagerController'
    };

    //                                 ------- add controllers -------
    var editTemplate = {
        name: 'mainView.templatesManager.editTemplate',
        url: '/editTemplate/{templateId:int}',
        templateUrl: 'Content/templates/templatesManager/templatesEdit.html'
    };
    var addTemplate = {
        name: 'mainView.templatesManager.addTemplate',
        url: '/addTemplate',
        templateUrl: 'Content/templates/templatesManager/templatesEdit.html',
        controller: 'AddTemplateController'
    };

    $stateProvider.state(templatesManager);
    $stateProvider.state(templatesList);
    $stateProvider.state(editTemplate);
    $stateProvider.state(addTemplate);
    $stateProvider.state(templateDetailsView);

}]);