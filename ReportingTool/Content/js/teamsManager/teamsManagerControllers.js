'use strict';

teamsManagerModule.controller('teamsManagerController',
    ['$scope', '$state', 'TeamFactory', 'TempObjectFactory', function ($scope, $state, TeamFactory, TempObjectFactory) {
        $scope.message = "Loading...";
        $scope.showTeams = true;
        $scope.teams = {};
        $scope.activeTeam = {};
        $scope.validationIsInProgress = true;

        TeamFactory.GetAllTeams().then(teamsSuccess, teamsFail);

        var DeleteTeam = null;

        $scope.deleteTeam = function (deletedTeamID) {
            TeamFactory.deleteTeam(deletedTeamID).then(delSuccess, delFail);
        }

        $scope.showTeamMembers = function (selectedTeam) {
            $scope.activeTeam = selectedTeam;
        }

        $scope.editTeam = function (updateTeam) {
            TempObjectFactory.set(updateTeam);
            $scope.activeTeam = {};
            $state.go('mainView.teamsManager.editTeam');
        }

        function delSuccess(response) {
            if (response.data.Answer == 'Deleted') {
                $state.go('mainView.teamsManager', {}, { reload: true });
            }
        }

        function delFail(response) {
            console.error("Delete failed!");
        }

        function teamsSuccess(response) {
            $scope.teams = response.data;
            $scope.validationIsInProgress = false;
            $scope.showTeams = true;
        }

        function teamsFail(response) {
            $scope.validationIsInProgress = false;
            alert("Error: " + response.code + " " + response.statusText);
        }

    }]);

teamsManagerModule.controller('teamsEditController',
    ['$scope', '$stateParams', '$state', 'TeamFactory', 'UserFactory', 'TempObjectFactory',
        function ($scope, $stateParams, $state, TeamFactory, UserFactory, TempObjectFactory) {

            $scope.jiraUsers = [{
                loginName: 'Loading...',
                fullName: 'Loading...'
            }];

            $scope.editTeam = TempObjectFactory.get();

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
                TempObjectFactory.set({});
            }

            $scope.cancel = function () {
                TempObjectFactory.set({});
                $state.go('mainView.teamsManager');
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

        }]);

teamsManagerModule.controller('NewTeamController',
    ['$scope', '$state', 'TeamFactory', 'UserFactory', function ($scope, $state, TeamFactory, UserFactory) {
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
            $state.go('mainView.teamsManager');
        }

        $scope.removeMember = function (userName) {
            for (var i in $scope.editTeam.members) {
                if ($scope.editTeam.members[i].userName === userName) {
                    $scope.editTeam.members.splice(i, 1);
                }
            }
        }

        function createSuccess(response) {
            if (response.data.Answer == 'Created') {
                $state.go('mainView.teamsManager', {}, { reload: true });
            }
        }

        function createFail(response) {
            console.error('create team fail!');
        }
    }]);
