'use strict';

manageTeamsModule
	.controller('TeamsController', ['$scope', 'TeamFactory', function ($scope, TeamFactory) {
	    $scope.message = "Loading...";
	    debugger;
		$scope.showTeams = false;
		$scope.teams = {};
		TeamFactory.all().then(teamsSuccess, teamsFail);

		var id_team_to_del = null;

		$scope.delTeam = function (id) {
			TeamFactory.del(id).then(delSuccess, delFail);
			id_team_to_del = id;
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
			$scope.message = "Error: " + response.code + " " + response.statusText;
		}

    }])
	.controller('EditTeamController', ['$scope', '$stateParams', '$state', 'TeamFactory', 'UserFactory', function ($scope, $stateParams, $state, TeamFactory, UserFactory) {
		$scope.editTeam = {
			teamName: ""
		};
		$scope.users = [{
			loginName: 'Loading...',
			fullName: 'Loading...'
		}];
		$scope.selectedUser = {
			userID: "",
			loginName: "",
			fullName: ""
		};
		$scope.message = "Loading...";
		$scope.showEditBlock = false;

		var id_team_to_del = parseInt($stateParams.id, 10);
		var backupTeam = {};

		TeamFactory.get(parseInt($stateParams.id, 10)).then(getSuccess, getFail);
		UserFactory.all().then(getUsersSuccess, getUsersFail);

		$scope.addUser = function (user) {
			for (var i in $scope.editTeam.members) {
				if ($scope.editTeam.members[i].userID === user.userID) {
					return;
				}
			}
			$scope.editTeam.members.push(user);
			$scope.selectedUser = {
				userID: "",
				loginName: "",
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

		$scope.delMember = function (id) {
			console.log('del member' + id);
			for (var i in $scope.editTeam.members) {
				if ($scope.editTeam.members[i].userID === id) {
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
		loginName: 'Loading...',
		fullName: 'Loading...'
	}];
	$scope.selectedUser = {
		userID: "",
		loginName: "",
		fullName: ""
	};
	$scope.showEditBlock = true;

	UserFactory.all().then(getUsersSuccess, getUsersFail);

	$scope.addUser = function (user) {
		for (var i in $scope.editTeam.members) {
			if ($scope.editTeam.members[i].userID === user.userID) {
				return;
			}
		}
		$scope.editTeam.members.push(user);
		$scope.selectedUser = {
			userID: "",
			loginName: "",
			fullName: ""
		};
	}

	$scope.save = function (editedTeam) {
		TeamFactory.create(editedTeam).then(createSuccess, createFail);
	}

	$scope.cancel = function () {
		$scope.editTeam = {
			teamID: "",
			teamName: "Python Django team",
			members: []
		};
	}

	$scope.delMember = function (id) {
		for (var i in $scope.editTeam.members) {
			if ($scope.editTeam.members[i].userID === id) {
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