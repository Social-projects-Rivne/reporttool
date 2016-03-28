'use strict';

var mainViewModule = angular.module('mainViewModule',
    ['teamsManagerModule',
        'templatesManagerModule',
        'reportsManagerModule',
        'exportReportModule',
        'ui.router']);

mainViewModule
    .config(['$stateProvider',
                    '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider) {
        var mainView = {
            name: 'mainView',
            url: 'ReportingTool',
            templateUrl: 'Content/templates/mainView.html',
            controller: function ($scope, TempObjectFactory) {
                    TempObjectFactory.set({});
            }
        }

        $stateProvider.state(mainView);

        $urlRouterProvider.otherwise('');
    }]);