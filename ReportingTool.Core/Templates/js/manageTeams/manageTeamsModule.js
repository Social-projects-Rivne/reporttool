'use strict'

var manageTeamsModule = angular.module("manageTeamsModule", ['ui.router'])
	.config(function ($stateProvider, $urlRouterProvider) {
		$stateProvider
			.state('teams', {
				url: 'teams',
				views: {
					'list': {
						templateUrl: 'templates/team_list.html',
						controller: 'TeamsController'
					},
					'edit_team': {
						template: '',
					}
				}
			})

		.state('teams.edit', {
				url: 'teams/:id',
				views: {
					'edit_team@': {
						templateUrl: 'templates/editteam.html',
						controller: 'EditTeamController'
					}
				}
			})
			.state('teams.new', {
				url: 'teams/new',
				views: {
					'edit_team@': {
						templateUrl: 'templates/editteam.html',
						controller: 'NewTeamController'
					}
				}
			});

		$urlRouterProvider.otherwise('teams');
	});