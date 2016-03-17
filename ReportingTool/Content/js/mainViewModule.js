'use strict';

var mainViewModule = angular.module('mainViewModule',
    ['teamsManagerModule',
        'templatesManagerModule',
        'reportsManagerModule',
        'ui.router']);

mainViewModule
    .config(['$stateProvider',
                    '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider) {
        var mainView = {
            name: 'mainView',
            url: 'ReportingTool',
            templateUrl: 'Content/templates/mainView.html',
        }

        $stateProvider.state(mainView);

        $urlRouterProvider.otherwise('');
    }]);