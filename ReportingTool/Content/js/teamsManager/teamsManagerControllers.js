'use strict';

teamsManagerModule
	.controller('teamsManagerController', ['$scope', '$stateParams', '$state', 'TeamFactory', function ($scope,$stateParams,$state, TeamFactory) {
	    $scope.message = "Loading...";
		$scope.showTeams = true;
		$scope.teams = {};
		TeamFactory.GetAllTeams().then(teamsSuccess, teamsFail);

		var id_team_to_del = null;

		$scope.delTeam = function (id) {
			TeamFactory.del(id).then(delSuccess, delFail);
			id_team_to_del = id;
		}

		$scope.AddTeam = function () {
		    $state.go('.createTeam');
		}

		function delSuccess(response) {
			for (var i in $scope.teams) {
				if ($scope.teams[i].teamID === id_team_to_del) {
					$scope.teams.splice(i, 1);
				}
			}
		}

		function delFail(response) {
			console.error("Epic fail!");
			// delete with working backend
			for (var i in $scope.teams) {
				if ($scope.teams[i].teamID === id_team_to_del) {
					$scope.teams.splice(i, 1);
				}
			}
		}

		function teamsSuccess(response) {
			$scope.teams = response.data;
			$scope.showTeams = true;
		}

		function teamsFail(response) {
			alert("Error: " + response.code + " " + response.statusText);
		}

    }])
	.controller('EditTeamController', ['$scope', '$stateParams', '$state', 'TeamFactory', 'UserFactory', function ($scope, $stateParams, $state, TeamFactory, UserFactory) {
		$scope.editTeam = {
			teamName: ""
		};
		$scope.users = [{
			userName: 'Loading...',
			fullName: 'Loading...'
		}];
		$scope.selectedUser = {
			userName: "",
			fullName: ""
		};
		$scope.message = "Loading...";
		$scope.showEditBlock = true;

		var id_team_to_del = parseInt($stateParams.id, 10);
		var backupTeam = {};

		TeamFactory.get(parseInt($stateParams.id, 10)).then(getSuccess, getFail);
		UserFactory.all().then(getUsersSuccess, getUsersFail);

		$scope.addUser = function (member) {
			for (var i in $scope.editTeam.members) {
			    if ($scope.editTeam.members[i].userName === user.userName) {
					return;
				}
			}
			$scope.editTeam.members.push(member);
			$scope.selectedUser = {
				userName: "",
				fullName: ""
			};
		}

		$scope.save = function (editedTeam) {
			TeamFactory.update(editedTeam).then(updateSuccess, updateFail);
		}

		$scope.cancel = function () {
			console.log($scope.editTeam + " = " + backupTeam);
			$scope.editTeam = backupTeam;
		}

		$scope.removeMember = function (userName) {
		    console.log('del member ' + userName);
			for (var i in $scope.editTeam.members) {
			    if ($scope.editTeam.members[i].userName === userName) {
					$scope.editTeam.members.splice(i, 1);
				}
			}
		}

		function updateSuccess(response) {
			$state.go('teams');
		}

		function updateFail(response) {
			console.error("error during saving edited team");
		}

		function getSuccess(response) {
			//delete with working backend:
			var teams = response.data;
			for (var team in teams) {
				if (teams[team].teamID === parseInt($stateParams.id, 10)) {
					$scope.editTeam = angular.copy(teams[team]);
					backupTeam = angular.copy(teams[team]);
					$scope.showEditBlock = true;
					$scope.message = "";
				}
			}
			// remain this:
			// $scope.editTeam = response.data
			// backupTeam = angular.copy(teams[team]);
			// $scope.showEditBlock = true;
		}

		function getFail(response) {
			$scope.message = "Loading team error! " + response.code;
		}

		function getUsersSuccess(response) {
			$scope.users = response.data;
		}

		function getUsersFail(response) {
			console.error("getUsers error!");
		}

    }])

.controller('NewTeamController', ['$stateParams', '$scope', '$state', 'TeamFactory', 'UserFactory', function ($stateParams, $scope, $state, TeamFactory, UserFactory) {
	$scope.editTeam = {
		teamID: "",
		teamName: "Python Django team",
		members: []
	};
	$scope.users = [{
		userName: 'Loading...',
		fullName: 'Loading...'
	}];
	$scope.selectedUser = {
		userName: "",
		fullName: ""
	};
	$scope.showEditBlock = true;

	UserFactory.all().then(getUsersSuccess, getUsersFail);

	$scope.addUser = function (user) {
		for (var i in $scope.editTeam.members) {
		    if ($scope.editTeam.members[i].userName === user.userName) {
				return;
			}
		}
		$scope.editTeam.members.push(user);
		$scope.selectedUser = {
			userName: "",
			fullName: ""
		};
	}

	$scope.save = function (editedTeam) {
		TeamFactory.create(editedTeam).then(createSuccess, createFail);
	}

	$scope.cancel = function () {
		$scope.editTeam = {
			teamID: "",
			teamName: "",
			members: []
		};
	}

	$scope.removeMember = function (userName) {
		for (var i in $scope.editTeam.members) {
		    if ($scope.editTeam.members[i].userName === userName) {
				$scope.editTeam.members.splice(i, 1);
			}
		}
	}

	function getUsersSuccess(response) {
		$scope.users = response.data;
	}

	function getUsersFail(response) {
		console.error("getUsers error!");
	}

	function createSuccess(response) {
		$state.go('teams');
	}

	function createFail(response) {
		console.error('create team fail!');
		$state.go('teams');
	}
    }])