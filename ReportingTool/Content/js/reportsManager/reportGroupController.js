'use strict';

reportsManagerModule.controller('reportGroupController',
    ['$scope', '$state', 'GroupFactory',
        function ($scope, $state, GroupFactory) {

            // an array of groups in the report
            $scope.reportedGroups = [];
            //$scope.groups = {};
            $scope.activeGroup = {};
                        
            //  a work template for group manipulation
            $scope.editGroup = {
                //groupID: "0",
                groupName: "",
                members: []
            };

            //  initial array of members for report
            $scope.reportedMembers = [{
                userName: 'Loading...',
                fullName: 'Loading...',
                role: 'No Role'
            }];

            function getAllMembers() {
                //var teams = $http.get("Teams/GetAllTeams");
                //var members = $scope.reportedMembers;
                var tmp_members = [];
                var tmp_member = {};

                for (var i in $scope.selectedTeam.members) {
                    tmp_member.userName = scope.selectedTeam.members[i].userName;
                    tmp_member.fullName = scope.selectedTeam.members[i].fullName;
                    tmp_member.role = '';

                    tmp_members.push(member);
                }
                tmp_member = {};
                return tmp_members;
            }

            $scope.reportedMembers = getAllMembers();

            //  a work template for member manipulation
            $scope.selectedMember = {
                userName: "",
                fullName: "",
                role: ""
            };

            //  $scope.message = "Loading...";
            //  $scope.showGroups = true;
            //  $scope.validationIsInProgress = true;

            //  ?
            $scope.tempReport = TempObjectFactory.get();
            TempObjectFactory.set({});

            // === from teamsManagerController ========================
            GroupFactory.getAllGroups()
                .then(groupsSuccess, groupsFail);
                //  TODO
                function groupsSuccess(response) {
                    //$scope.groups = response.data;
                    //$scope.validationIsInProgress = false;
                    //$scope.showGroups = true;
                }
                //  TODO
                function groupsFail(response) {
                    //$scope.validationIsInProgress = false;
                    //alert("Error: " + response.code + " " + response.statusText);
                }
            //  --------------------------------------------------------------------------------------
                var DeleteGroup = null;

            $scope.deleteGroup = function (deleteGroupName) {
                GroupFactory
                    .deleteGroup(deleteGroupName)
                    .then(deleteGroupSuccess, deleteGroupFail);
            }
                 //  TODO
                function deleteGroupSuccess(response) {
                        //if (response.data.Answer == 'Deleted') {
                        //    $state.go('mainView.teamsManager', {}, { reload: true });
                        //}
                }
                function deleteGroupFail(response) {
                        console.error("Delete failed!");
                 }
            //  --------------------------------------------------------------------------------------
            $scope.showGroupMembers = function (selectedGroup) {
                $scope.activeGroup = selectedGroup;
            }

            //  TODO : a view for Group edit ? - maybe add + delete will be enough
            $scope.editGroup = function (updateGroup) {

                TempObjectFactory.set(updateGroup);
                $scope.activeGroup = {};

                //  TODO : a view for Group edit ? - maybe add + delete will be enough
                //  $state.go('mainView.groupsManager.editGroup');
            }
            // ======================================================

            // === from teamsEditController =============================
            $scope.groupUsers = [{
                loginName: 'Loading...',
                fullName: 'Loading...'
            }];

            $scope.editGroup = TempObjectFactory.get();

            //  --------------------------------------------------------------------------------------
            GroupFactory.getAllMembers()
                .then(getAllMembersSuccess, getAllMembersFail);

                function getAllMembersSuccess(response) {
                    //$scope.groupUsers = response.data;
                }
                function getAllMembersFail(response) {
                    console.error("getAllMembers fail !");
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
                //  add the member to the group's list 
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
                console.log('delete member : ' + userName);

                for (var i in $scope.editGroup.members) {
                    if ($scope.editGroup.members[i].userName === userName) {
                        $scope.editGroup.members.splice(i, 1);
                    }
                }

                //  add the member to reportedMembers array
                $scope.reportedMembers.push(member);

            }

            $scope.save = function () {
                GroupFactory.updateGroup($scope.editGroup)
                    .then(updateSuccess, updateFail);

                TempObjectFactory.set({});
            }
                function updateSuccess(response) {
                    //$state.go('mainView.teamsManager', {}, { reload: true });
                    $state.go('mainView.reportsManager.reportsConditions.reportDraft', {}, { reload: true });
                }
                function updateFail(response) {
                    console.error("error during saving edited group");
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
                GroupFactory.createGroup($scope.editGroup)
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
                console.log('delete member : ' + userName);

                for (var i in $scope.editGroup.members) {
                    if ($scope.editGroup.members[i].userName === userName) {
                        $scope.editGroup.members.splice(i, 1);
                    }
                }

                //  add the member back to reportedMembers array
                $scope.reportedMembers.push(member);

            }

            //  =====================================================

        }]);
