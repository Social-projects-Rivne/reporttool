﻿'use strict'

var configurationModule = angular.module("configurationModule", ['ui.router']);

configurationModule.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
    var configView = {
        name: 'configView',
        url: 'Configuration',
        templateUrl: 'Content/templates/configView.html',
        controller: 'configurationController'
    }

    $stateProvider.state(configView);
}]);