var dbm = global.dbm || require('db-migrate');
var type = dbm.dataType;

//exports.up = function(db, callback) {
//  callback();
//};

//exports.down = function(db, callback) {
//  callback();
//};

exports.up = function (db, callback) {
    db.createTable('pets', {
        id: { type: 'serial', primaryKey: true },
        name: 'string'
    }, callback);
};

exports.down = function (db, callback) {
    db.dropTable('pets', callback);
};


