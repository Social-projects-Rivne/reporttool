"use strict"

var ReportingTool = angular.module('ReportingTool', ['ui.router', 'mainViewModule', 'configurationModule', 'loginModule']);

ReportingTool.run(['$rootScope', '$state', '$stateParams',
    function ($rootScope,   $state,   $stateParams) {
        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;

   $state.transitionTo('configView');
}]);