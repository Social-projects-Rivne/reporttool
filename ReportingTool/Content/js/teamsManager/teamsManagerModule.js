'use strict'

var teamsManagerModule = angular.module("teamsManagerModule", ['ui.router']);

teamsManagerModule.config(['$stateProvider', function ($stateProvider, $urlRouterProvider) {

    var teamManager = {
        name: 'mainView.teamsManager',
        url: '/teamsManager',

        views: {
            '': { templateUrl: 'Content/templates/teamsManager/teamsManagerView.html' },
            'teamsList@mainView.teamsManager': {
                templateUrl: 'Content/templates/teamsManager/teamsList.html'
            }
        }

    }


    $stateProvider.state(teamManager);
}]);




//var teamsManager = {
//    name: 'mainView.teamsManager',
//    url: '/teamsManager',
//        view: {
//            '': { templateUrl: 'Content/templates/teamsManager/teamsManagerView.html' },
//            'teamsList@teamsManager': {
//                templateUrl: 'Content/templates/teamsManager/teamsList.html'
//            },
//            'editBlock@teamsManager': {
//                template: '',
//            }

//        }
//    },
//        editTeam = {
//            name: 'teamsManager.editTeam',
//            url: '/editTeam/:teamID',
//            view: {
//                'editBlock@': {
//                    templateUrl: 'Content/templates/teamsManager/editTeam.html'
//                }
//            }
//        },
//        createTeam = {
//            name: 'teamsManager.createTeam',
//            url: '/createTeam',
//            view: { 'editBlock@': { templateUrl: 'Content/templates/teamsManager/editTeam.html' } }
//        };

//    $stateProvider.state(teamsManager);
//    $stateProvider.state(editTeam);
//    $stateProvider.state(createTeam);
//}]);











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