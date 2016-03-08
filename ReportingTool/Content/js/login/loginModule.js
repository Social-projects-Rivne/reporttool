'use strict'

var loginModule = angular.module("loginModule", ['ngMaterial', 'ngMessages', 'ui.router', 'ui.bootstrap', 'ngAnimate']);

loginModule.config(['$stateProvider', '$urlRouterProvider',
    function ($stateProvider, $urlRouterProvider, $animateProvider) {
    var loginView = {
        name: 'loginView',
        url: 'Login',
        templateUrl: 'Content/templates/loginView.html',
        controller: 'loginController'
    }

        var loginView = {
            name: 'loginView',
            url: 'Login',
            templateUrl: 'Content/templates/loginView.html',
            controller: 'loginController'
        }

        $stateProvider.state(loginView);
    }]);