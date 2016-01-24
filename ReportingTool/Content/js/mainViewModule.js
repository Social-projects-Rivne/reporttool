'use strict'

var mainViewModule = angular.module('mainViewModule', ['teamsManagerModule', 'ui.router']);

mainViewModule.config(['$stateProvider', function ($stateProvider) {
    var mainView = {
        name: 'mainView',
        url: '/ReportingTool',
        templateUrl: 'Content/templates/mainView.html',
        //controller: 'MainViewController' // it should get all teams from server and store it to some rootScope variable
    }

    $stateProvider.state(mainView);
}]);