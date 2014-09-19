//1、	数据库链接信息
var db_config = {
    host: '10.5.24.73',
    user: 'root',
    password: '95938',
    database: 'chat_test',
    multipleStatements: true,
    connectTimeout: 1000 * 1000
};

//2、	站点链接信息
var websiteConfig = {
    CgZtbWeb: {
        host: '10.5.24.73',
        port: '8098',
        path: "http://10.5.24.73:8098/"
    },
    mysite: "http://10.5.24.73:5858/"
};

//1、	数据库链接信息
exports.db_config = db_config;

//2、	站点链接信息
exports.websiteConfig = websiteConfig;



