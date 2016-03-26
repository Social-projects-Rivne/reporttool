/// <reference path="../vendor/angular.js" />
'use strict';

reportsManagerModule.controller("reportsManagerController",
    ['$scope', '$stateParams', '$state','ReportsFactory', 'TempObjectFactory','$filter',
        function ($scope, $stateParams, $state, ReportsFactory, TempObjectFactory,  $filter) {

            var tempTemplate = TempObjectFactory.get();
            $scope.content = "team";
            $scope.templateValue = tempTemplate.templateName;

            $scope.teams = [{
                teamName: 'Loading...',
                teamID:''
            }];

            $scope.templates = [{
                templateName: 'Loading...',
                templateId: ''
            }];

            $scope.selectedTeam = {};
            $scope.selectedTemplate = {};

            ReportsFactory.getTeams().then(function(response) {
                $scope.teams = response.data;
            }, function(error) {
                console.error("getTeams error!", error);
            });
 
            ReportsFactory.getTemplates().then(function (response) {
                $scope.templates = response.data;
            }, function (error) {
                console.error("getTemplates error!", error);
            });

            $scope.fromDate = new Date();
            $scope.toDate = new Date();

            $scope.clear = function () {
                $scope.from = null;
                $scope.to = null;
            };

            $scope.dateOptions = {
                dateDisabled: disabled,
                maxDate: new Date(2020, 5, 22),
                minDate: new Date(1990, 12, 31),
                startingDay: 1
            };

            // Disable weekend selection
            function disabled(data) {
                var date = data.date,
                  mode = data.mode;
                return mode === 'day' && (date.getDay() === 0 || date.getDay() === 6);
            }

            $scope.open1 = function () {
                $scope.popup1.opened = true;              
            };

            $scope.open2 = function () {
                $scope.popup2.opened = true;              
            };

            $scope.setDate = function (year, month, day) {
                $scope.dt = new Date(year, month, day);
            };

            $scope.popup1 = {
                opened: false
            };

            $scope.popup2 = {
                opened: false
            };

            function formatDate(date) {
                //return $filter('date')(date, "dd/MM/yyyy");   //  fix
                return $filter('date')(date, "yyyy-MM-dd");
            }

            $scope.saveIntoClientStorage = function () {
                //TODO: Add here validation about all input values and selected data
                //TODO: Add checking if all template data haven't come with tempTemplate
                var dataPromise = ReportsFactory.getFields($scope.selectedTemplate.templateId);
                dataPromise.then(function(result) {
                    var tempReportConditionals = {
                        //templateName: $scope.selectedTemplate.templateName,
                        //fields: fields,
                        //template: getFields($scope.selectedTemplate.templateId),
                        template: result.data,
                        teamId: $scope.selectedTeam.teamID,
                        from: formatDate($scope.fromDate),
                        to: formatDate($scope.toDate)
                    };
                    TempObjectFactory.set({});
                    TempObjectFactory.set(tempReportConditionals);
                    $state.go('mainView.reportsManager.reportsConditions.reportDraft');
                });           
            }
        }]);

reportsManagerModule.controller("reportDraftController",
    ['$scope', '$stateParams', '$state','ReportsFactory', 'TempObjectFactory',
        function ($scope, $stateParams, $state, ReportsFactory, TempObjectFactory) {

            $scope.tempReport = TempObjectFactory.get();
            TempObjectFactory.set({});

            //  -------------------------------------------------------------------------   //  debug
            // --- code from templatesFieldsManagerController ---
            //  $scope.templateData = {};
            //  $scope.templateId = $stateParams.templateId;
            //  -------------------------------------------------------------------------   //  debug

            $scope.fields = getFields();

            //  $scope.existData = false;
            //  $scope.isOwner = false;
            //  $scope.validationIsInProgress = true;

            //  1
            //function getFields() {
            //    //  TemplateFactory.GetAllTemplateFields($scope.templateId)
            //    //ReportsFactory.getFields($scope.selectedTemplate.templateId) 
            //    ReportsFactory.getTemplateFields($scope.selectedTemplate.templateId)
            //    .then(function (data) {
            //        templateFieldsSuccess(data);
            //    }, function (error) {
            //        templateFieldsFail(error);
            //    });
            //}

            //  2
            function getFields() {
                //  TemplateFactory.GetAllTemplateFields($scope.templateId)
                //ReportsFactory.getFields($scope.selectedTemplate.templateId) 
                ReportsFactory.getTemplateFields($scope.selectedTemplate.templateId)
                .then(function (data) {
                    templateFieldsSuccess(data);
                }, function (error) {
                    templateFieldsFail(error);
                });
            }

            function templateFieldsSuccess(data) {
                // promise fulfilled
                $scope.templateData = data;
                $scope.fields = $scope.templateData.data.fields;
                //$scope.existData = true;
                //$scope.isOwner = data.IsOwner;
                //$scope.validationIsInProgress = false;
            };

            function templateFieldsFail(error) {
                // promise rejected, could log the error with: console.log('error', error);
                //  $scope.existData = true;    //  fix
                //  $scope.validationIsInProgress = false;
                console.log("Error: " + error.code + " " + error.statusText);
            };

            /*
            $scope.getFieldValue = function (field) {
                if (field.fieldDefaultValue)
                    return field.fieldDefaultValue;
                else {
                    if (field.fieldName.toLowerCase() === fieldEnum.RisksAndIssues.toLowerCase() ||
                        field.fieldName.toLowerCase() === fieldEnum.PlannedActivities.toLowerCase() ||
                        field.fieldName.toLowerCase() === fieldEnum.UserActivities.toLowerCase()) {
                        return $scope.fieldValueGeneratedAutomatically;
                    } else {
                        return $scope.fieldValueSetManually;
                    }
                }
            }
            */

            //  ========================================================================
            // an array of groups in the report
            $scope.reportedGroups = [];

            //  $scope.groups = {};
            //  $scope.activeGroup = {};

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
                role: 'No Role',
                activity: '2h',
                issues: []
            }];

            //  a number of async requests for progres indication
            //$scope.requestProcessing = false;
            $scope.requestProcessing = 0;

            $scope.addGroupPanel = false;

            function getAllMembers() {
                //var teams = $http.get("Teams/GetAllTeams");
                //var members = $scope.reportedMembers;
                var tmp_members = [];

                //for (var i in $scope.selectedTeam.members) {
                for (var i = 0, len = $scope.selectedTeam.members.length; i < len; i++) {
                    var tmp_member = {};
                    tmp_member.userName = $scope.selectedTeam.members[i].userName;
                    tmp_member.fullName = $scope.selectedTeam.members[i].fullName;
                    tmp_member.role = '';
                    tmp_member.activity = 0;

                    tmp_members.push(tmp_member);
                }
                return tmp_members;
            }
            $scope.reportedMembers = getAllMembers();

            //  not done and not called
            function getActivityAndIssues() {
                for (var i = 0, len = $scope.reportedMembers.length; i < len; i++) {

                    $scope.getUserActivity($scope.reportedMembers[i].userName, $scope.tempReport.from, $scope.tempReport.to);
                    //$scope.getIssues($scope.reportedMembers[i].userName, $scope.tempReport.from, $scope.tempReport.to);
                }
            }

            //  a work template for member manipulation
            $scope.selectedMember = {
                userName: "",
                fullName: "",
                role: "",
                activity: "",
                issues: []
            };

            // add a member to a new group being set up
            $scope.addMember = function (member) {
                //  check to avoid member duplication
                for (var i in $scope.editGroup.members) {
                    if ($scope.editGroup.members[i].userName === member.userName) {
                        return;
                    }
                }
                //  add the member to the group's list 
                var tmp_member = {};
                console.log("Adding member : " + member);
                tmp_member.userName = member.userName;
                tmp_member.fullName = member.fullName;
                tmp_member.role = member.role;
                tmp_member.activity = member.activity;
                // adding issues
                tmp_member.issues = member.issues;

                //$scope.editGroup.members.push(member);
                $scope.editGroup.members.push(tmp_member);

                //  remove the added member from initial report list
                for (var i in $scope.reportedMembers) {
                    if ($scope.reportedMembers[i].userName === member.userName) {
                        $scope.reportedMembers.splice(i, 1);
                        //  $scope.editGroup.members.splice(i, 1);
                    }
                }
            }

            //  remove a member from a new group being set up
            $scope.removeEditGroupMember = function (userName) {
                console.log('del member ' + userName);
                for (var i in $scope.editGroup.members) {
                    if ($scope.editGroup.members[i].userName === userName) {
                        var tmp_member = {};

                        tmp_member.userName = $scope.editGroup.members[i].userName;
                        tmp_member.fullName = $scope.editGroup.members[i].fullName;
                        tmp_member.role = $scope.editGroup.members[i].role;
                        tmp_member.activity = $scope.editGroup.members[i].activity;
                        //  get the issues back
                        tmp_member.issues = $scope.editGroup.members[i].issues;

                        $scope.reportedMembers.push(tmp_member);
                        
                        $scope.editGroup.members.splice(i, 1);
                    }
                }
            }

            //  a new group being set up : save
            $scope.save = function () {
                var tmp_group = {
                    groupName: "",
                    members: []
                };
                tmp_group.groupName = $scope.editGroup.groupName;

                //  remove the added member from initial report list
                for (var i in $scope.editGroup.members) {
                    for (var j in $scope.reportedMembers) {
                        if ($scope.editGroup.members[i].userName === $scope.reportedMembers[j].userName) {
                            $scope.reportedMembers.splice(j, 1);
                        }
                    }
                    tmp_group.members.push($scope.editGroup.members[i]);
                }

                //$scope.reportedGroups.push($scope.editGroup);
                $scope.reportedGroups.push(tmp_group);

                //  clear the editGroup obj
                $scope.editGroup.groupName = "";
                $scope.editGroup.members = [];

                $scope.addGroupPanel = false;
                //  $state.go('mainView.reportsManager.reportsConditions.reportDraft', {}, { reload: true });
            }

            //  a new group being set up : cancel 
            $scope.cancel = function () {
                //  write the cancelled group members back to initial report list
                for (var i in $scope.editGroup.members) {
                    var tmp_member = {};

                    tmp_member.userName = $scope.editGroup.members[i].userName;
                    tmp_member.fullName = $scope.editGroup.members[i].fullName;
                    tmp_member.role = $scope.editGroup.members[i].role;
                    tmp_member.activity = $scope.editGroup.members[i].activity;
                    //  get the issues back
                    tmp_member.issues = $scope.editGroup.members[i].issues;

                    $scope.reportedMembers.push(tmp_member);
                    $scope.editGroup.members.splice(i, 1);
                }
                console.log("group save cancelled");
                $scope.addGroupPanel = false;
                //  $state.go('mainView.reportsManager.reportsConditions.reportDraft', {}, { reload: true });
            }

            //  remove a group 
            $scope.removeGroup = function (groupId) {
                for (var i in $scope.reportedGroups[groupId].members) {
                    var tmpMember = $scope.reportedGroups[groupId].members[i];

                    $scope.reportedMembers.push(tmpMember);
                    //$scope.reportedGroups[groupId].members.splice(groupId, 1);
                }
                $scope.reportedGroups.splice(groupId, 1);
            };

            //  IN PROGRESS : remove a member from a group
            $scope.removeGroupMember = function (userName) {
                console.log('removeGroupMember : ' + userName);
                for (var j in $scope.reportedGroups) {
                    for (var i in $scope.reportedGroups[j].members) {

                        if ($scope.reportedGroups[j].members[i].userName === userName) {
                            var tmp_member = {};

                            tmp_member.userName = $scope.reportedGroups[j].members[i].userName;
                            tmp_member.fullName = $scope.reportedGroups[j].members[i].fullName;
                            tmp_member.role = $scope.reportedGroups[j].members[i].role;
                            tmp_member.activity = $scope.reportedGroups[j].members[i].activity;
                            //  get the issues back
                            tmp_member.issues = $scope.reportedGroups[j].members[i].issues;

                            $scope.reportedMembers.push(tmp_member);

                            $scope.reportedGroups[j].members.splice(i, 1);
                            return;
                        }
                    }
                }

            }

            $scope.groupMemberList = function (group) {
                var tmpMemberList = "";

                for (var i in group.members) {
                    if (tmpMemberList.length > 0) {
                        tmpMemberList += ", ";
                    }
                    tmpMemberList += group.members[i].fullName ;
                }
                tmpMemberList = "(" + tmpMemberList + ")";
                return tmpMemberList;
            };

            //  in progress !!! - is there any need ?
            $scope.groupIssueArray = function (group) {
                var tmpIssueArray = [];

                for (var i in  $scope.reportedGroups) {
                    for (var j in group.members.issues) {
                        var tmpIssue = group.members[i].issues[j];
                        tmpIssueArray.push(tmpIssue);
                    }
                }
                console.log("tmpIssueArray = " + tmpIssueArray);

                return tmpIssueArray;
            };

            $scope.getUserActivity = function (userName, dateFrom, dateTo) {

                //  debug
                console.log("userName = " + userName);
                console.log("dateFrom = " + dateFrom);
                console.log("dateTo = " + dateTo);

                //  hung up the browser
                //ReportsFactory.getUserActivity(userName, dateFrom, dateTo)
                //    .then(function (response) {
                //        //$scope.teams = response.data;
                //        userActivity = response.data;
                //    }, function (error) {
                //        console.error("getUserActivity error!", error);
                //    });

                //$scope.requestProcessing = true;
                $scope.requestProcessing += 1;

                ReportsFactory.getUserActivityRequest(userName, dateFrom, dateTo)
                .then(gUASuccess, gUAFail);

            };
                function gUASuccess(response) {
                    //$scope.requestProcessing = false;
                    $scope.requestProcessing -= 1;

                    //  fill the activity field on scope
                    // $scope.time = (parseInt(response.data.Timespent) / 3600);
                    var seconds = response.data.Timespent;
                    var hours = Math.round( parseInt( response.data.Timespent) / 3600);
                    var userNameVar = response.data.userNameFromBE;

                    for (var i in $scope.reportedMembers) {
                        if ($scope.reportedMembers[i].userName === userNameVar) {
                            $scope.reportedMembers[i].activity = hours;
                        }
                    }

                }
                function gUAFail(response) {
                    //$scope.requestProcessing = false;
                    $scope.requestProcessing -= 1;

                    alert("Error: " + response.code + ".  " + response.statusText);
                }
            //$scope.Test02 = $scope.getUserActivity("user1", "date1", "date2");

            $scope.getIssues = function (userName, dateFrom, dateTo) {
                //$scope.requestProcessing = true;
                $scope.requestProcessing += 1;

                ReportsFactory.getIssuesRequest(userName, dateFrom, dateTo)
                .then(gISuccess, gIFail);

            };
                function gISuccess(response) {
                    //$scope.requestProcessing = false;
                    $scope.requestProcessing -= 1;

                    //  fill the issues field on scope
                    var issuesVar = response.data.Issues;
                    var userNameVar = response.data.userNameFromBE;

                    for (var i in $scope.reportedMembers) {
                        if ($scope.reportedMembers[i].userName === userNameVar) {
                            $scope.reportedMembers[i].issues = issuesVar;
                        }
                    }
                }
                function gIFail(response) {
                    //$scope.requestProcessing = false;
                    $scope.requestProcessing -= 1;

                    alert("Error: " + response.code + ".  " + response.statusText);
                }
            //$scope.Test03 = $scope.getIssues("user1", "date1", "date2");

            //  TODO
                function getReportedMembersInfo() {
                    var from = $scope.tempReport.from;
                    var to = $scope.tempReport.to;

                    for (var i = 0, len = $scope.reportedMembers.length; i < len; i++) {

                        //  launch the activity Ajax request
                        var username = $scope.reportedMembers[i].userName;
                        //var from = $scope.tempReport.from;
                        //var to = $scope.tempReport.to;

                        $scope.getUserActivity(username, from, to);
                        $scope.getIssues(username, from, to);
                    }
                }
                getReportedMembersInfo();

            //  test User Activity for a fixed user
                //$scope.TestActivity = function () {
                //    console.log("TestActivity start");
                //        ReportsFactory.TestActivityRequest();
                //        console.log("TestActivity end");
                //        return 1;
                //}
                //$scope.TActivity = $scope.TestActivity();

            //  worked OK !
            //$scope.Test = ReportsFactory.getUserActivityRequest("user1", "date1", "date2");
                        

            //  ========================================================================


        }]);

