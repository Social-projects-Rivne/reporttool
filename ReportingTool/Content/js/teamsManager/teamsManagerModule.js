'use strict'

var teamsManagerModule = angular.module("teamsManagerModule", ['ui.router']);

teamsManagerModule.config(['$stateProvider', function ($stateProvider) {
    var teamsManager = {
        name: 'teamsManager',
        url: 'main/teamsManager',
        templateUrl: 'Content/templates/teamsManager/teamsManagerView.html',
        controller: 'teamsManagerController'
    },
       teamsList = {
           name: 'teamsManager.teamsList',
           url: 'main/teamsManager/teamsList',
           templateUrl: 'Content/templates/teamsManager/teamsList.html',
           parrent: 'teamsManager'
       },
       editTeam = {
           name: 'teamsManager.editTeam',
           url: 'main/teamsManager/editTeam/:teamID',
           templateUrl: 'Content/templates/teamsManager/editTeam.html',
           parrent: 'teamsManager'
       },
       createTeam = {
           name: 'teamsManager.createTeam',
           url: 'main/teamsManager/createTeam',
           templateUrl: 'Content/templates/teamsManager/editTeam.html',
           parrent: 'teamsManager'
       };

    $stateProvider.state(teamsManager);
    $stateProvider.state(teamsList);
    $stateProvider.state(editTeam);
    $stateProvider.state(createTeam);
}]);











//manageTeamsModule.config(function ($stateProvider, $urlRouterProvider) {
//    $stateProvider
//        .state('teams', {
//            url: 'teams',
//            views: {
//                'list': {
//                    templateUrl: 'Home/templates/team_list.html',
//                    controller: 'TeamsController'
//                },
//                'edit_team': {
//                    template: '',
//                }
//            }
//        })

//    .state('teams.edit', {
//        url: 'teams/:id',
//        views: {
//            'edit_team@': {
//                templateUrl: 'templates/editteam.html',
//                controller: 'EditTeamController'
//            }
//        }
//    })
//        .state('teams.new', {
//            url: 'teams/new',
//            views: {
//                'edit_team@': {
//                    templateUrl: 'templates/editteam.html',
//                    controller: 'NewTeamController'
//                }
//            }
//        });

//    $urlRouterProvider.otherwise('teams');
//});