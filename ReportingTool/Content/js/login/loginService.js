'use strict';

loginModule.service('LoginService', function () {
   // alert("service created");
    this.showLogout = { show: false };
    this.showLogin = { show: false };

    this.SetShowLogoutStatus =  function (showLogout) {
        this.showLogout.show = showLogout;
    };

    this.GetShowLogoutStatus = function() {
        return this.showLogout;
    };

    this.SetShowLoginStatus = function (showLogin) {
        this.showLogin.show = showLogin;
    };

    this.GetShowLoginStatus = function () {
        return this.showLogin;
    };

});


