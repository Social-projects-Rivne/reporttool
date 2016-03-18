'use strict';

reportsManagerModule.controller('reportGroupController',
    ['$scope', '$state', 'GroupFactory',
        function ($scope, $state, GroupFactory) {

            //  initial array of members for report
            $scope.reportedMembers = [];

            // an array of groups in the report
            $scope.reportedGroups = [];
            $scope.groups = {};
            $scope.activeGroup = {};

            //  a work template for group manipulation
            $scope.editGroup = {
                //groupID: "0",
                groupName: "",
                members: []
            };

            //  a work template for member manipulation
            $scope.selectedMember = {
                userName: '',
                fullName: ''
            };

            //  $scope.message = "Loading...";
            //  $scope.showTeams = true;
            //  $scope.validationIsInProgress = true;

            $scope.tempReport = TempObjectFactory.get();
            TempObjectFactory.set({});

            // === from teamsManagerController ========================
            GroupFactory.GetAllGroups()
                .then(groupsSuccess, groupsFail);

                function groupsSuccess(response) {
                    //$scope.groups = response.data;
                    //$scope.validationIsInProgress = false;
                    //$scope.showGroups = true;
                }
                function groupsFail(response) {
                    //$scope.validationIsInProgress = false;
                    alert("Error: " + response.code + " " + response.statusText);
                }
            //  --------------------------------------------------------------------------------------
            var DeleteGroup = null;

            $scope.deleteGroup = function (deletedGroupID) {
                GroupFactory
                    .deleteGroup(deletedGroupID)
                    .then(delSuccess, delFail);
            }

                function delSuccess(response) {
                    //if (response.data.Answer == 'Deleted') {
                    //    $state.go('mainView.teamsManager', {}, { reload: true });
                    //}
                }
                function delFail(response) {
                    console.error("Delete failed!");
                }
            //  --------------------------------------------------------------------------------------
            $scope.showGroupMembers = function (selectedGroup) {
                $scope.activeGroup = selectedGroup;
            }

            $scope.editGroup = function (updateGroup) {
                TempObjectFactory.set(updateGroup);
                $scope.activeGroup = {};

                //  TODO : a view for Group edit ? - maybe add + delete will be enough
                $state.go('mainView.groupsManager.editGroup');
            }
            // ======================================================

            // === from teamsEditController =============================
            $scope.groupUsers = [{
                loginName: 'Loading...',
                fullName: 'Loading...'
            }];

            $scope.editGroup = TempObjectFactory.get();

            $scope.selectedMember = {
                userName: '',
                fullName: ''
            }
            //  --------------------------------------------------------------------------------------
            GroupFactory.getAllMembers()
                .then(getAllMembersSuccess, getAllMembersFail);

                function getAllMembersSuccess(response) {
                    $scope.groupUsers = response.data;
                }
                function $scope.groupUsersFail(response) {
                    console.error("getUsers error!");
                };
            //  --------------------------------------------------------------------------------------
            $scope.message = "Loading...";
            $scope.showEditBlock = true;

            $scope.addMember = function (member) {
                //  check to avoid member duplication
                for (var i in $scope.editGroup.members) {
                    if ($scope.editGroup.members[i].userName === member.userName) {
                        return;
                    }
                }
                $scope.editGroup.members.push(member);

                //  remove the added member from initial report list
                for (var i in $scope.reportedMembers) {
                    if ($scope.reportedMembers[i].userName === member.userName) {
                        $scope.reportedMembers.splice(i, 1);
                        //  $scope.editGroup.members.splice(i, 1);
                    }
                }
            }

            $scope.removeMember = function (userName) {
                console.log('del member ' + userName);

                for (var i in $scope.editGroup.members) {
                    if ($scope.editGroup.members[i].userName === userName) {
                        $scope.editGroup.members.splice(i, 1);
                    }
                }
            }

            $scope.save = function () {
                GroupFactory.updateTeam($scope.editTeam)
                    .then(updateSuccess, updateFail);

                TempObjectFactory.set({});
            }
                function updateSuccess(response) {
                    //$state.go('mainView.teamsManager', {}, { reload: true });
                    $state.go('mainView.reportsManager.reportsConditions.reportDraft', {}, { reload: true });
                }
                function updateFail(response) {
                    console.error("error during saving edited team");
                }

                $scope.cancel = function () {
                    TempObjectFactory.set({});

                    //$state.go('mainView.teamsManager');
                    $state.go('mainView.reportsManager.reportsConditions.reportDraft', {}, { reload: true });
            }
          
            // ======================================================

            // ======================================================

            //  === from NewTeamController ============================
            //  $scope.showEditBlock = true;
            //  DONE : add a member to the group
            $scope.addMember = function (member) {
                //  check to avoid member duplication
                for (var i in $scope.editGroup.members) {
                    if ($scope.editGroup.members[i].userName === member.userName) {
                        return;
                    }
                }
                $scope.editGroup.members.push(member);

                //  remove the added member from initial report list
                for (var i in $scope.reportedMembers) {
                    if ($scope.reportedMembers[i].userName === member.userName) {
                        $scope.reportedMembers.splice(i, 1);
                        //  $scope.editGroup.members.splice(i, 1);
                    }
                }
            }
            //  --------------------------------------------------------------------------------------
            //  DONE : group creation saved
            $scope.save = function () {
                GroupFactory.createGroup($scope.editGrup)
                    .then(createSuccess, createFail);
            }
                //  DONE : create group success
                function createSuccess(response) {
                    //if (response.data.Answer == 'Created') {
                    //    $state.go('mainView.teamsManager', {}, { reload: true });
                    //}

                    $state.go('mainView.reportsManager.reportsConditions.reportDraft', {}, { reload: true });
                }
                //  DONE : create group fail
                function createFail(response) {
                    console.error('create group fail!');
                }
            //  --------------------------------------------------------------------------------------
            //  DONE : group creation cancelled
            $scope.cancel = function () {
                $scope.editGroup = {
                    //groupID: "",
                    groupName: "",
                    members: []
                };
                //$state.go('mainView.teamsManager');
                $state.go('mainView.reportsManager.reportsConditions.reportDraft');
            }
            //  --------------------------------------------------------------------------------------
            //  DONE : remove a member from the group
            $scope.removeMember = function (userName) {
                for (var i in $scope.editGroup.members) {
                    if ($scope.editGroup.members[i].userName === userName) {
                        $scope.editGroup.members.splice(i, 1);
                    }
                }
            }
            //  =====================================================

        }]);
