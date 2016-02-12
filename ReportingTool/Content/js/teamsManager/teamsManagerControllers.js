'use strict';

teamsManagerModule.controller('teamsManagerController',
    ['$scope', '$state', 'TeamFactory', 'JiraUsersService', function ($scope, $state, TeamFactory, JiraUsersService) {
        $scope.message = "Loading...";
        $scope.showTeams = true;
        $scope.teams = {};
        $scope.activeTeam = {};
        
        TeamFactory.GetAllTeams().then(teamsSuccess, teamsFail);

        JiraUsersService.GetJiraUsersFromServer();

        var DeleteTeam = null;

        $scope.deleteTeam = function (id) {
            TeamFactory.deleteTeam(id).then(delSuccess, delFail);
            id_team_to_del = id;
        }

        $scope.teamSelect = function (selectedTeam) {
            $scope.activeTeam = selectedTeam;
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

    }]);
	
teamsManagerModule.controller('EditTeamController',
    ['$scope', '$stateParams', '$state', 'TeamFactory', 'JiraUsersService', function ($scope, $stateParams, $state, TeamFactory, JiraUsersService)
    {
	    $scope.editTeam = {
            teamID: "",
            teamName: "",
            members: []
	    };

	    $scope.selectedMember = {
	        UserName: '',
	        FullName: ''
	    }

	    $scope.jiraUsers = JiraUsersService.getJiraUsers;

	    $scope.message = "Loading...";
	    $scope.showEditBlock = true;

	    var id_team_to_del = parseInt($stateParams.id, 10);
	    var backupTeam = {};


	    $scope.addMember = function (member) {
	        for (var i in $scope.editTeam.members) {
	            if ($scope.editTeam.members[i].UserName === member.UserName) {
	                return;
	            }
	        }
	        $scope.editTeam.members.push(member);
	    }

	    $scope.save = function (editedTeam) {
	        TeamFactory.update(editedTeam).then(updateSuccess, updateFail);
	    }

	    $scope.cancel = function () {
	        console.log($scope.editTeam + " = " + backupTeam);
	        $scope.editTeam = backupTeam;
	    }

	    $scope.removeMember = function (UserName) {
	        console.log('del member ' + UserName);
	        for (var i in $scope.editTeam.members) {
	            if ($scope.editTeam.members[i].UserName === UserName) {
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

	}]);

teamsManagerModule.controller('NewTeamController',
    ['$stateParams', '$scope', '$state', 'TeamFactory', 'JiraUsersService', function ($stateParams, $scope, $state, TeamFactory, JiraUsersService) {
        $scope.editTeam = {
            teamID: "0",
            teamName: "",
            members: []
        };

        $scope.selectedMember = {
            UserName: '',
            FullName: ''
        };


        $scope.jiraUsers = JiraUsersService.getJiraUsers();

        $scope.showEditBlock = true;

        $scope.addMember = function (member) {
            for (var i in $scope.editTeam.members) {
                if ($scope.editTeam.members[i].UserName === member.UserName) {
                    //---------------- TODO: Add duplicates exception ----------------------//
                    return;
                }
            }
            $scope.editTeam.members.push(member);
        }

        $scope.save = function () {
            TeamFactory.createTeam($scope.editTeam).then(createSuccess, createFail);
        }

        $scope.cancel = function () {
            $scope.editTeam = {
                teamID: "",
                teamName: "",
                members: []
            };
        }

        $scope.removeMember = function (UserName) {
            for (var i in $scope.editTeam.members) {
                if ($scope.editTeam.members[i].UserName === UserName) {
                    $scope.editTeam.members.splice(i, 1);
                }
            }
        }

        function createSuccess(response) {
            $state.go('mainView.teamsManager');
        }

        function createFail(response) {
            console.error('create team fail!');
        }
    }]);