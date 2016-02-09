'use strict'

var teamsManagerModule = angular.module("teamsManagerModule", ['ui.router']);

teamsManagerModule.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

    var teamsManager = {
        name: 'mainView.teamsManager',
        url: '/teamsManager',

        views: {
            '': {
                templateUrl: 'Content/templates/teamsManager/teamsManagerView.html'
            },
            'teamsList@mainView.teamsManager': {
                templateUrl: 'Content/templates/teamsManager/teamsList.html'
            }
        }
    };

    var editTeam = {
        name: 'mainView.teamsManager.editTeam',
        url: '/editTeam/:teamID',
        templateUrl: 'Content/templates/teamsManager/editTeam.html'
    };
    var createTeam = {
        name: 'mainView.teamsManager.createTeam',
        url: '/createTeam',
        templateUrl: 'Content/templates/teamsManager/editTeam.html',
        controller: 'NewTeamController'
    };

    $stateProvider.state(teamsManager);
    $stateProvider.state(editTeam);
    $stateProvider.state(createTeam);
}]);