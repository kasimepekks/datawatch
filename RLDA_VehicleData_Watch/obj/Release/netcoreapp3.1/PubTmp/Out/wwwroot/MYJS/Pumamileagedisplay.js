


layui.use(['element', 'layer', 'laydate', 'table', 'form'], function () {
    var $ = layui.jquery;
    layer = layui.layer;
  /*  var element = layui.element;*/
    var table = layui.table;
  
    
/*    var datevalue, startdate, enddate;*/
    var a = GetRequest();
   

    //先查看有没有分析权限
    $.ajax({

        type: "POST",
        //请求的媒体类型
        dataType: 'text',
        //请求地址
        url: urlgetvehicleparadata,
        data: {
            vehicleid: a['id']
        },

        success: function (data) {
            if (data != "yes") {
                layer.msg(data);
            }

        }
        , error: function (e) {
            layer.msg(e);
        }
    });
 
    $("#pumamileagedatadisplay").click(function () {

        var tableIns = table.render(
            {
                elem: '#pumamileagedata'
                , toolbar: '#toolbarDemo'
                ,defaultToolbar: ['filter', 'exports', 'print']//添加打印按钮
                , url: urlgetpumamileagedata //数据接口
                , limit: 30
                , even: true
                , page: true //开启分页
                , size: 'sm'
                ,where: { //传递参数
                    vehicle: a['id'],
                }
                , cols: [[ //表头

                    { field: 'id', title: '序号', sort: true }

                    , { field: 'vehicle', title: '车辆编号', sort: true }
                    , { field: 'mileage', title: '里程', sort: true }

                    , { field: 'duration', title: '持续时间', sort: true }
                    , { field: 'averagespeed', title: '平均车速', sort: true }
                    , { field: 'maxthrottle', title: '最大油门', sort: true }
                    , { field: 'maxbrake', title: '最大刹车', sort: true }
                    , { field: 'maxangle', title: '最大转角', sort: true }
                    , { field: 'minangle', title: '最小转角', sort: true }
                    , { field: 'wftfxlfk3', title: '左前X损伤k3', templet: function (d) { return parseFloat(d.wftfxlfk3).toFixed(10).toString(); }}
                    , { field: 'wftfxrfk3', title: '右前X损伤k3', templet: function (d) { return parseFloat(d.wftfxrfk3).toFixed(10).toString(); }}
                    , { field: 'wftfxlrk3', title: '左后X损伤k3', templet: function (d) { return parseFloat(d.wftfxlrk3).toFixed(10).toString(); }}
                    , { field: 'wftfxrrk3', title: '右后X损伤k3', templet: function (d) { return parseFloat(d.wftfxrrk3).toFixed(10).toString(); }}
                    , { field: 'wftfylfk3', title: '左前Y损伤k3', templet: function (d) { return parseFloat(d.wftfylfk3).toFixed(10).toString(); }}
                    , { field: 'wftfyrfk3', title: '右前Y损伤k3', templet: function (d) { return parseFloat(d.wftfyrfk3).toFixed(10).toString(); }}
                    , { field: 'wftfylrk3', title: '左后Y损伤k3', templet: function (d) { return parseFloat(d.wftfylrk3).toFixed(10).toString(); }}
                    , { field: 'wftfyrrk3', title: '右后Y损伤k3', templet: function (d) { return parseFloat(d.wftfyrrk3).toFixed(10).toString(); }}
                    , { field: 'wftfzlfk3', title: '左前Z损伤k3', templet: function (d) { return parseFloat(d.wftfzlfk3).toFixed(10).toString(); }}
                    , { field: 'wftfzrfk3', title: '右前Z损伤k3', templet: function (d) { return parseFloat(d.wftfzrfk3).toFixed(10).toString(); }}
                    , { field: 'wftfzlrk3', title: '左后Z损伤k3', templet: function (d) { return parseFloat(d.wftfzlrk3).toFixed(10).toString(); }}
                    , { field: 'wftfzrrk3', title: '右后Z损伤k3', templet: function (d) { return parseFloat(d.wftfzrrk3).toFixed(10).toString(); }}
                    , { field: 'wftfxlfk5', title: '左前X损伤k5', templet: function (d) { return parseFloat(d.wftfxlfk5).toFixed(10).toString(); }}
                    , { field: 'wftfxrfk5', title: '右前X损伤k5', templet: function (d) { return parseFloat(d.wftfxrfk5).toFixed(10).toString(); }}
                    , { field: 'wftfxlrk5', title: '左后X损伤k5', templet: function (d) { return parseFloat(d.wftfxlrk5).toFixed(10).toString(); }}
                    , { field: 'wftfxrrk5', title: '右后X损伤k5', templet: function (d) { return parseFloat(d.wftfxrrk5).toFixed(10).toString(); }}
                    , { field: 'wftfylfk5', title: '左前Y损伤k5', templet: function (d) { return parseFloat(d.wftfylfk5).toFixed(10).toString(); }}
                    , { field: 'wftfyrfk5', title: '右前Y损伤k5', templet: function (d) { return parseFloat(d.wftfyrfk5).toFixed(10).toString(); }}
                    , { field: 'wftfylrk5', title: '左后Y损伤k5', templet: function (d) { return parseFloat(d.wftfylrk5).toFixed(10).toString(); }}
                    , { field: 'wftfyrrk5', title: '右后Y损伤k5', templet: function (d) { return parseFloat(d.wftfyrrk5).toFixed(10).toString(); }}
                    , { field: 'wftfzlfk5', title: '左前Z损伤k5', templet: function (d) { return parseFloat(d.wftfzlfk5).toFixed(10).toString(); }}
                    , { field: 'wftfzrfk5', title: '右前Z损伤k5', templet: function (d) { return parseFloat(d.wftfzrfk5).toFixed(10).toString(); }}
                    , { field: 'wftfzlrk5', title: '左后Z损伤k5', templet: function (d) { return parseFloat(d.wftfzlrk5).toFixed(10).toString(); }}
                    , { field: 'wftfzrrk5', title: '右后Z损伤k5', templet: function (d) { return parseFloat(d.wftfzrrk5).toFixed(10).toString(); }}
                ]]
            });
        table.on('toolbar(pumamileagedata)', function (obj) {
            switch (obj.event) {

                case 'exportcsv':
                    $.ajax({
                        type: 'POST',
                        url: saveAllPumamileagedata,
                        data: {
                            vehicle: a['id']
                        },
                        success: function (res) {
                            if (res == "Yes") {
                                //处理导出结果，可以根据需要自行设置
                                layer.msg("导出成功，请在根目录下查看mileagecsv.csv");
                            }
                            else {
                                layer.msg("导出失败");
                            }
                            //console.log(res);
                        }
                    });
                    break;
            };
        });

    })

    });
