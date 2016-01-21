'use strict'

var manageTeamsModule = angular.module("manageTeamsModule", ['ui.router']);

manageTeamsModule.run([function () {
    alert('Im here!');
}]);

manageTeamsModule.config(['$stateProvider', function ($stateProvider) {
    var main = {
        name: 'main',
        url: '/main',
        templateUrl: 'Views/Shared/templates/mainView.html'
        //controller: 'loginController'
    };
    $stateProvider.state(main);
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