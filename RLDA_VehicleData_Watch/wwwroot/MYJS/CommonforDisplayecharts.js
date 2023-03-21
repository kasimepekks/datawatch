
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = decodeURIComponent(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}

//document.write('<link href="../../layui/css/layui.css" rel="stylesheet" />');
//document.write('<script src="../../layui/layui.js"></script>');

document.write('<script src="../../lib/microsoft/signalr/dist/browser/signalr.js"></script>');
document.write('<script type="text/javascript" src="//api.map.baidu.com/api?v=3.0&ak=FFvnSXHMHaC0CQgTguKY17zkEwwfqjsP"></script>');

document.write('<script src="../../MYJS/GPSConverttoBaidu.js"></script>');
document.write('<script src="../../MYJS/LuShu_min.js"></script>');
var urlfilewatcher = '/FileWatch/FileWatch';
