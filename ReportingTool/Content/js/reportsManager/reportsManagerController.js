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
                return $filter('date')(date, "dd/MM/yyyy");
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
                role: 'No Role',
                activity: '2h'
            }];

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
                    tmp_member.activity = '0h'

                    tmp_members.push(tmp_member);
                }
                return tmp_members;
            }

            $scope.reportedMembers = getAllMembers();

            //  a work template for member manipulation
            $scope.selectedMember = {
                userName: "",
                fullName: "",
                role: "",
                activity: ""
            };

            $scope.addMember = function (member) {
                //  check to avoid member duplication
                for (var i in $scope.editGroup.members) {
                    if ($scope.editGroup.members[i].userName === member.userName) {
                        return;
                    }
                }
                //  add the member to the group's list 
                var tmp_member = {};
                console.log(member);
                tmp_member.userName = member.userName;
                tmp_member.fullName = member.fullName;
                tmp_member.role = member.role;
                tmp_member.activity = member.activity;

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

            $scope.cancel = function () {
                //  write the cancelled group members back to initial report list
                for (var i in $scope.editGroup.members) {
                    var tmp_member = {};

                    tmp_member.userName = $scope.editGroup.members[i].userName;
                    tmp_member.fullName = $scope.editGroup.members[i].fullName;
                    tmp_member.role = $scope.editGroup.members[i].role;
                    tmp_member.activity = $scope.editGroup.members[i].activity;

                    $scope.reportedMembers.push(tmp_member);
                    $scope.editGroup.members.splice(i, 1);
                }
                console.log("group save cancelled");
                $scope.addGroupPanel = false;
                //  $state.go('mainView.reportsManager.reportsConditions.reportDraft', {}, { reload: true });
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
            }

            //  ========================================================================


        }]);

