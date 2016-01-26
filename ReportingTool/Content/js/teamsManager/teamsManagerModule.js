'use strict'

var teamsManagerModule = angular.module("teamsManagerModule", ['ui.router']);

teamsManagerModule.config(['$stateProvider', function ($stateProvider, $urlRouterProvider) {

    var teamsManager = {
        name: 'mainView.teamsManager',
        url: '/teamsManager',

        views: {
            '': { templateUrl: 'Content/templates/teamsManager/teamsManagerView.html' },
            'teamsList@mainView.teamsManager': {
                templateUrl: 'Content/templates/teamsManager/teamsList.html'
            }
        }

    };

    var editTeam = {
        name: 'mainView.teamsManager.editTeam',
        url: '/editTeam/:teamID',
        view: {
            'editBlock@mainView.teamsManager': {
                templateUrl: 'Content/templates/teamsManager/editTeam.html'
            }
        }
    };
    var createTeam = {
        name: 'mainView.teamsManager.createTeam',
        url: '/createTeam',
        view: { 'editBlock@mainView.teamsManager': { templateUrl: 'Content/templates/teamsManager/editTeam.html' } }
    };

    $stateProvider.state(teamsManager);
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