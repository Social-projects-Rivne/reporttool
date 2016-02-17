'use strict';

teamsManagerModule.controller('teamsManagerController',
    ['$scope', '$state', 'TeamFactory', 'TempTeamFactory', function ($scope, $state, TeamFactory, TempTeamFactory) {
        $scope.message = "Loading...";
        $scope.showTeams = true;
        $scope.teams = {};
        $scope.activeTeam = {};

        TeamFactory.GetAllTeams().then(teamsSuccess, teamsFail);

        var DeleteTeam = null;

        $scope.deleteTeam = function (id) {
            TeamFactory.deleteTeam(id).then(delSuccess, delFail);
            id_team_to_del = id;
        }

        $scope.showTeamMembers = function (selectedTeam) {
            $scope.activeTeam = selectedTeam;
        }

        $scope.editTeam = function (updateTeam) {
            TempTeamFactory.setTempTeam(updateTeam);
            $scope.activeTeam = {};
            $state.go('mainView.teamsManager.editTeam');
        }

        function delSuccess(response) {
            for (var i in $scope.teams) {
                if ($scope.teams[i].teamID === id_team_to_del) {
                    $scope.teams.splice(i, 1);
                }
            }
        }

        function delFail(response) {
            console.error("Delete failed!");
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
    ['$scope', '$stateParams', '$state', 'TeamFactory', 'UserFactory', 'TempTeamFactory',
        function ($scope, $stateParams, $state, TeamFactory, UserFactory, TempTeamFactory) {

            $scope.jiraUsers = [{
                loginName: 'Loading...',
                fullName: 'Loading...'
            }];

            $scope.editTeam = TempTeamFactory.getTempTeam();

            $scope.selectedMember = {
                userName: '',
                fullName: ''
            }

            UserFactory.getJiraUsers().then(getJiraUsersSuccess, getJiraUsersFail);

            function getJiraUsersSuccess(response) {
                $scope.jiraUsers = response.data;
            }

            function getJiraUsersFail(response) {
                console.error("getUsers error!");
            };

            $scope.message = "Loading...";
            $scope.showEditBlock = true;

            var id_team_to_del = parseInt($stateParams.id, 10);
            var backupTeam = {};


            $scope.addMember = function (member) {
                for (var i in $scope.editTeam.members) {
                    if ($scope.editTeam.members[i].userName === member.userName) {
                        return;
                    }
                }
                $scope.editTeam.members.push(member);
            }

            $scope.save = function () {
                TeamFactory.updateTeam($scope.editTeam).then(updateSuccess, updateFail);
                TempTeamFactory.setTempTeam({});
            }

            $scope.cancel = function () {
                console.log($scope.editTeam + " = " + backupTeam);
                $scope.editTeam = backupTeam;
                TempTeamFactory.setTempTeam({});
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
                $state.go('mainView.teamsManager');
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
    ['$stateParams', '$scope', '$state', 'TeamFactory', 'UserFactory', function ($stateParams, $scope, $state, TeamFactory, UserFactory) {
        $scope.editTeam = {
            teamID: "0",
            teamName: "",
            members: []
        };

        $scope.selectedMember = {
            userName: '',
            fullName: ''
        };

        $scope.jiraUsers = [{
            userName: 'Loading...',
            fullName: 'Loading...'
        }];

        UserFactory.getJiraUsers().then(getJiraUsersSuccess, getJiraUsersFail);

        function getJiraUsersSuccess(response) {
            $scope.jiraUsers = response.data;
        }

        function getJiraUsersFail(response) {
            console.error("getUsers error!");
        };

        $scope.showEditBlock = true;

        $scope.addMember = function (member) {
            for (var i in $scope.editTeam.members) {
                if ($scope.editTeam.members[i].userName === member.userName) {
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

        $scope.removeMember = function (userName) {
            for (var i in $scope.editTeam.members) {
                if ($scope.editTeam.members[i].userName === userName) {
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