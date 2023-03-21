var a = GetRequest();
var connection;
//var routpath = [];
var polyline;
var filename;
var title;
var chanindex = [];
var channame = [];
var chart_series = [];
var chart_series_final = [];
var chart_series_name = [];
$("#pagetitle").text(a['id'] + "监控页面");

connection = new signalR.HubConnectionBuilder().withUrl("/MyHub").build();
connection.serverTimeoutInMilliseconds = 30000;
connection.keepAliveIntervalInMilliseconds = 15000;




function filewatchcontroller() {
  $.ajax({

    type: "POST",
    //请求的媒体类型
    dataType: 'text',//这里改为json就不会传回success需要的数据了
    //请求地址
    url: urlfilewatcher,
    data: {
        vehicleid: a['id']
    },
    success: function (data) {
        title = data;
        $("#title").text(title);
        if (window.navigator.onLine) {
            if (title.indexOf("国外") > 0) {
                map.setMapStyleV2({
                    styleId: 'cdba3d9358345e622a2cebe3665b7a40'
                });
                myIcon = new BMap.Icon('/Pictures/marker.png',
                    new BMap.Size(10, 10), {
                    anchor: new BMap.Size(0, 0)
                });
            }
            else {
                myIcon = new BMap.Icon('/Pictures/marker_red.png',
                    new BMap.Size(10, 10), {
                    anchor: new BMap.Size(0, 0)
                });
            }
        }
       
     }
  });
}
function sum(arr) {
    return arr.reduce(function (total, value) {
        return total + value;
    }, 0);
}//数组求和
filewatchcontroller();

connection.on("SpeedtoDistance", function (_vehicleID, distance, speed, Lat, Lon,readtimes) {
   
  if (_vehicleID == a['id']) {
      document.querySelector(".showreadtimes").innerHTML = readtimes;
    //判断服务器传过来的是哪辆车就显示哪辆车的信息，因为每辆车的数据源不一样
      $("#speed").text(speed);
      $("#milege").text(distance.toFixed(2));

      //先判断是否有经纬度，如没有则不用添加轨迹在地图上
      if (sum(Lat) > 0) {
          if (navigator.onLine) {
              var allPoint = [];
              var testPoint = [];
              //只取第一个和最后一个经纬度
              for (var i = 0; i < Lat.length; i++) {
                  if (i == 0 || i == Lat.length - 1) {
                      allPoint.push(new BMap.Point(Lon[i], Lat[i]));
                  }
              }
              //百度坐标转换的回调函数
              callback = function (xyResult) {

                  for (var i = 0; i < xyResult.length; i++) {
                      testPoint.push(new BMap.Point(xyResult[i]["x"], xyResult[i]["y"]));

                  }
                  //for (var i = 0; i < testPoint.length; i++) {
                  //    var marker = new BMap.Marker(testPoint[i], { icon: myIcon });
                  //    map.addOverlay(marker);
                  //}
                  var distance = map.getDistance(testPoint[0], testPoint[1]);
                  if (distance < 250) {
                      polyline = new BMap.Polyline(testPoint, {
                          strokeColor: "yellow",
                          strokeWeight: 6,
                          strokeOpacity: 0.7
                      });
                  }
                 
                  map.addOverlay(polyline);

                  routpath = routpath.concat(testPoint);
                  map.setViewport(routpath);
                  if (lushu == null) {
                      lushu = new BMapLib.LuShu(map, testPoint, {
                          /* defaultContent: _vehicleID,*/
                           autoView: true, //是否开启自动视野调整，如果开启那么路书在运动过程中会根据视野自动调整
                           icon: mycarIcon,
                          enableRotation: true, //是否设置marker随着道路的走向进行旋转
                          speed: 2000, //速度很有关系，太快和太慢都会导致车标原地不动
                          landmarkPois: []
                      });
                      lushu.start();

                  }

                  else {


                      lushu.goPath(testPoint)
                  }


              }

              BMap.Convertor.transMore(allPoint, 0, callback);

          }
      }
  }

});

connection.on("ReloadDataACC", function (_vehicleID, time, name, otherchannels, channelorderlist, echarttitle, echartdata) {
    
    if (_vehicleID == a['id']) {
        $("#filename").html(name);
        if (otherchannels.length > 0)
        {
            //先对每个图表标题赋值，最多6个
            if (echarttitle.length == 6) {
                $("#chart1_h2").html(echarttitle[0]);
                $("#chart2_h2").html(echarttitle[1]);
                $("#chart3_h2").html(echarttitle[2]);
                $("#chart4_h2").html(echarttitle[3]);
                $("#chart5_h2").html(echarttitle[4]);
                $("#chart6_h2").html(echarttitle[5]);
            }
            else {
                $("#chart1_h2").html("数据库图表标题有误");
                $("#chart2_h2").html("数据库图表标题有误");
                $("#chart3_h2").html("数据库图表标题有误");
                $("#chart4_h2").html("数据库图表标题有误");
                $("#chart5_h2").html("数据库图表标题有误");
                $("#chart6_h2").html("数据库图表标题有误");
            }
            chart_series_final = [];
            //0  "accxwhllf,accxwhlrf,accxwhllr,accxwhlrr"
            //console.log(echartdata);
            for (var i = 0; i < echartdata.length; i++) {
               
                //转为数组，e为每个图表的4个通道名
                var e = echartdata[i].split(",");
                //console.log(e);
                //存储所有的seriesname
                chart_series_name.push(...e);
                for (var j = 0; j < e.length;j++) {
                    //查找数组中每个元素在channelorderlist的index
                    var dataindex = channelorderlist.indexOf(e[j]);
                    for (var k= 0; k < time.length; k++) {
                        chart_series.push([time[k], otherchannels[dataindex][k]]);
                    }
                    
                    //把time和对应的通道全部放入到一个数组中，最终数组中有24个time和data组成的元素数组，每个元素有128个数据点
                    chart_series_final.push(chart_series);//这里不能加[],否则会变成2维数组，chart_series_final.push([chart_series]);
                    chart_series = [];//一定要清零，否则chart_series会越来越多
                   
                }
            }
           // console.log(chart_series_final);
            // 折线图1模块制作
            (function () {
                // 1. 实例化对象
                var myChart = echarts.init(document.querySelector(".line1 .chart"));

                // 2.指定配置
                var option = {
                    // 通过这个color修改两条线的颜色
                    color: ["#00f2f1", "#ed3f35", "#F8B448", "#8B78F6"],
                    tooltip: {
                        trigger: "axis"
                    },
                    legend: {
                        // 如果series 对象有name 值，则 legend可以不用写data
                        // 修改图例组件 文字颜色
                        textStyle: {
                            color: "#4c9bfd"
                        },
                        // 这个10% 必须加引号
                        right: "10%"
                    },
                    grid: {
                        top: "20%",
                        left: "3%",
                        right: "4%",
                        bottom: "3%",
                        show: true, // 显示边框
                        borderColor: "#012f4a", // 边框颜色
                        containLabel: true // 包含刻度文字在内
                    },

                    xAxis: {
                        type: "category",
                        boundaryGap: false,

                        axisTick: {
                            show: false // 去除刻度线
                        },
                        axisLabel: {
                            color: "#4c9bfd" // 文本颜色
                        },
                        axisLine: {
                            show: false // 去除轴线
                        }
                    },
                    yAxis: {
                        type: "value",
                        axisTick: {
                            show: false // 去除刻度线
                        },
                        axisLabel: {
                            color: "#4c9bfd" // 文本颜色
                        },
                        axisLine: {
                            show: false // 去除轴线
                        },
                        splitLine: {
                            lineStyle: {
                                color: "#012f4a" // 分割线颜色
                            }
                        }
                    },
                    series: [
                        {
                            name: chart_series_name[0],
                            type: "line",
                            // true 可以让我们的折线显示带有弧度
                            smooth: true,
                            data: chart_series_final[0]
                        },
                        {
                            name: chart_series_name[1],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[1]
                        },
                        {
                            name: chart_series_name[2],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[2]
                        },
                        {
                            name: chart_series_name[3],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[3]
                        }
                    ]
                };

                // 3. 把配置给实例对象
                myChart.setOption(option);
                // 4. 让图表跟随屏幕自动的去适应
                window.addEventListener("resize", function () {
                    myChart.resize();
                });

            })();
            // 折线图2模块制作
            (function () {

                // 1. 实例化对象
                var myChart = echarts.init(document.querySelector(".line2 .chart"));
                // 2.指定配置
                var option = {
                    // 通过这个color修改两条线的颜色
                    color: ["#00f2f1", "#ed3f35", "#F8B448", "#8B78F6"],
                    tooltip: {
                        trigger: "axis"
                    },
                    legend: {
                        // 如果series 对象有name 值，则 legend可以不用写data
                        // 修改图例组件 文字颜色
                        textStyle: {
                            color: "#4c9bfd"
                        },
                        // 这个10% 必须加引号
                        right: "10%"
                    },
                    grid: {
                        top: "20%",
                        left: "3%",
                        right: "4%",
                        bottom: "3%",
                        show: true, // 显示边框
                        borderColor: "#012f4a", // 边框颜色
                        containLabel: true // 包含刻度文字在内
                    },

                    xAxis: {
                        type: "category",
                        boundaryGap: false,

                        axisTick: {
                            show: false // 去除刻度线
                        },
                        axisLabel: {
                            color: "#4c9bfd" // 文本颜色
                        },
                        axisLine: {
                            show: false // 去除轴线
                        }
                    },
                    yAxis: {
                        type: "value",
                        axisTick: {
                            show: false // 去除刻度线
                        },
                        axisLabel: {
                            color: "#4c9bfd" // 文本颜色
                        },
                        axisLine: {
                            show: false // 去除轴线
                        },
                        splitLine: {
                            lineStyle: {
                                color: "#012f4a" // 分割线颜色
                            }
                        }
                    },
                    series: [
                        {
                            name: chart_series_name[4],
                            type: "line",
                            // true 可以让我们的折线显示带有弧度
                            smooth: true,
                            data: chart_series_final[4]
                        },
                        {
                            name: chart_series_name[5],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[5]
                        },
                        {
                            name: chart_series_name[6],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[6]
                        },
                        {
                            name: chart_series_name[7],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[7]
                        }
                    ]
                };

                // 3. 把配置给实例对象
                myChart.setOption(option);
                // 4. 让图表跟随屏幕自动的去适应
                window.addEventListener("resize", function () {
                    myChart.resize();
                });

            })();
            // 折线图3模块制作
            (function () {

                // 1. 实例化对象
                var myChart = echarts.init(document.querySelector(".line3 .chart"));
                // 2.指定配置
                var option = {
                    // 通过这个color修改两条线的颜色
                    color: ["#00f2f1", "#ed3f35", "#F8B448", "#8B78F6"],
                    tooltip: {
                        trigger: "axis"
                    },
                    legend: {
                        // 如果series 对象有name 值，则 legend可以不用写data
                        // 修改图例组件 文字颜色
                        textStyle: {
                            color: "#4c9bfd"
                        },
                        // 这个10% 必须加引号
                        right: "10%"
                    },
                    grid: {
                        top: "20%",
                        left: "3%",
                        right: "4%",
                        bottom: "3%",
                        show: true, // 显示边框
                        borderColor: "#012f4a", // 边框颜色
                        containLabel: true // 包含刻度文字在内
                    },

                    xAxis: {
                        type: "category",
                        boundaryGap: false,

                        axisTick: {
                            show: false // 去除刻度线
                        },
                        axisLabel: {
                            color: "#4c9bfd" // 文本颜色
                        },
                        axisLine: {
                            show: false // 去除轴线
                        }
                    },
                    yAxis: {
                        type: "value",
                        axisTick: {
                            show: false // 去除刻度线
                        },
                        axisLabel: {
                            color: "#4c9bfd" // 文本颜色
                        },
                        axisLine: {
                            show: false // 去除轴线
                        },
                        splitLine: {
                            lineStyle: {
                                color: "#012f4a" // 分割线颜色
                            }
                        }
                    },
                    series: [
                        {
                            name: chart_series_name[8],
                            type: "line",
                            // true 可以让我们的折线显示带有弧度
                            smooth: true,
                            data: chart_series_final[8]
                        },
                        {
                            name: chart_series_name[9],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[9]
                        },
                        {
                            name: chart_series_name[10],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[10]
                        },
                        {
                            name: chart_series_name[11],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[11]
                        }
                    ]
                };

                // 3. 把配置给实例对象
                myChart.setOption(option);
                // 4. 让图表跟随屏幕自动的去适应
                window.addEventListener("resize", function () {
                    myChart.resize();
                });

            })();

            // 折线图4模块制作
            (function () {

                // 1. 实例化对象
                var myChart = echarts.init(document.querySelector(".line4 .chart"));
                // 2.指定配置
                var option = {
                    // 通过这个color修改两条线的颜色
                    color: ["#00f2f1", "#ed3f35", "#F8B448", "#8B78F6"],
                    tooltip: {
                        trigger: "axis"
                    },
                    legend: {
                        // 如果series 对象有name 值，则 legend可以不用写data
                        // 修改图例组件 文字颜色
                        textStyle: {
                            color: "#4c9bfd"
                        },
                        // 这个10% 必须加引号
                        right: "10%"
                    },
                    grid: {
                        top: "20%",
                        left: "3%",
                        right: "4%",
                        bottom: "3%",
                        show: true, // 显示边框
                        borderColor: "#012f4a", // 边框颜色
                        containLabel: true // 包含刻度文字在内
                    },

                    xAxis: {
                        type: "category",
                        boundaryGap: false,

                        axisTick: {
                            show: false // 去除刻度线
                        },
                        axisLabel: {
                            color: "#4c9bfd" // 文本颜色
                        },
                        axisLine: {
                            show: false // 去除轴线
                        }
                    },
                    yAxis: {
                        type: "value",
                        axisTick: {
                            show: false // 去除刻度线
                        },
                        axisLabel: {
                            color: "#4c9bfd" // 文本颜色
                        },
                        axisLine: {
                            show: false // 去除轴线
                        },
                        splitLine: {
                            lineStyle: {
                                color: "#012f4a" // 分割线颜色
                            }
                        }
                    },
                    series: [
                        {
                            name: chart_series_name[12],
                            type: "line",
                            // true 可以让我们的折线显示带有弧度
                            smooth: true,
                            data: chart_series_final[12]
                        },
                        {
                            name: chart_series_name[13],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[13]
                        },
                        {
                            name: chart_series_name[14],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[14]
                        },
                        {
                            name: chart_series_name[15],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[15]
                        }
                    ]
                };

                // 3. 把配置给实例对象
                myChart.setOption(option);
                // 4. 让图表跟随屏幕自动的去适应
                window.addEventListener("resize", function () {
                    myChart.resize();
                });

            })();
            // 折线图5模块制作
            (function () {

                // 1. 实例化对象
                var myChart = echarts.init(document.querySelector(".line5 .chart"));
                // 2.指定配置
                var option = {
                    // 通过这个color修改两条线的颜色
                    color: ["#00f2f1", "#ed3f35", "#F8B448", "#8B78F6"],
                    tooltip: {
                        trigger: "axis"
                    },
                    legend: {
                        // 如果series 对象有name 值，则 legend可以不用写data
                        // 修改图例组件 文字颜色
                        textStyle: {
                            color: "#4c9bfd"
                        },
                        // 这个10% 必须加引号
                        right: "10%"
                    },
                    grid: {
                        top: "20%",
                        left: "3%",
                        right: "4%",
                        bottom: "3%",
                        show: true, // 显示边框
                        borderColor: "#012f4a", // 边框颜色
                        containLabel: true // 包含刻度文字在内
                    },

                    xAxis: {
                        type: "category",
                        boundaryGap: false,

                        axisTick: {
                            show: false // 去除刻度线
                        },
                        axisLabel: {
                            color: "#4c9bfd" // 文本颜色
                        },
                        axisLine: {
                            show: false // 去除轴线
                        }
                    },
                    yAxis: {
                        type: "value",
                        axisTick: {
                            show: false // 去除刻度线
                        },
                        axisLabel: {
                            color: "#4c9bfd" // 文本颜色
                        },
                        axisLine: {
                            show: false // 去除轴线
                        },
                        splitLine: {
                            lineStyle: {
                                color: "#012f4a" // 分割线颜色
                            }
                        }
                    },
                    series: [
                        {
                            name: chart_series_name[16],
                            type: "line",
                            // true 可以让我们的折线显示带有弧度
                            smooth: true,
                            data: chart_series_final[16]
                        },
                        {
                            name: chart_series_name[17],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[17]
                        },
                        {
                            name: chart_series_name[18],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[18]
                        },
                        {
                            name: chart_series_name[19],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[19]
                        }
                    ]
                };

                // 3. 把配置给实例对象
                myChart.setOption(option);
                // 4. 让图表跟随屏幕自动的去适应
                window.addEventListener("resize", function () {
                    myChart.resize();
                });

            })();
            // 折线图6模块制作
            (function () {

                // 1. 实例化对象
                var myChart = echarts.init(document.querySelector(".line6 .chart"));
                // 2.指定配置
                var option = {
                    // 通过这个color修改两条线的颜色
                    color: ["#00f2f1", "#ed3f35", "#F8B448", "#8B78F6"],
                    tooltip: {
                        trigger: "axis"
                    },
                    legend: {
                        // 如果series 对象有name 值，则 legend可以不用写data
                        // 修改图例组件 文字颜色
                        textStyle: {
                            color: "#4c9bfd"
                        },
                        // 这个10% 必须加引号
                        right: "10%"
                    },
                    grid: {
                        top: "20%",
                        left: "3%",
                        right: "4%",
                        bottom: "3%",
                        show: true, // 显示边框
                        borderColor: "#012f4a", // 边框颜色
                        containLabel: true // 包含刻度文字在内
                    },

                    xAxis: {
                        type: "category",
                        boundaryGap: false,

                        axisTick: {
                            show: false // 去除刻度线
                        },
                        axisLabel: {
                            color: "#4c9bfd" // 文本颜色
                        },
                        axisLine: {
                            show: false // 去除轴线
                        }
                    },
                    yAxis: {
                        type: "value",
                        axisTick: {
                            show: false // 去除刻度线
                        },
                        axisLabel: {
                            color: "#4c9bfd" // 文本颜色
                        },
                        axisLine: {
                            show: false // 去除轴线
                        },
                        splitLine: {
                            lineStyle: {
                                color: "#012f4a" // 分割线颜色
                            }
                        }
                    },
                    series: [
                        {
                            name: chart_series_name[20],
                            type: "line",
                            // true 可以让我们的折线显示带有弧度
                            smooth: true,
                            data: chart_series_final[20]
                        },
                        {
                            name: chart_series_name[21],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[21]
                        },
                        {
                            name: chart_series_name[22],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[22]
                        },
                        {
                            name: chart_series_name[23],
                            type: "line",
                            smooth: true,
                            data: chart_series_final[23]
                        }
                    ]
                };

                // 3. 把配置给实例对象
                myChart.setOption(option);
                // 4. 让图表跟随屏幕自动的去适应
                window.addEventListener("resize", function () {
                    myChart.resize();
                });

            })();
         
        }
    }


});

connection.start().then(function () {
    console.log("已开始监视");

}).catch(function (err) {
    setTimeout(() => start(), 10000);
    return console.log("首次链接: "+err.toString());
});

async function start() {
    try {
        await connection.start();
        console.log("reconnected");
    } catch (err) {
        console.log("reconnected: "+err);
        setTimeout(() => start(), 10000);
    }
};

connection.onclose(async (e) => {
    console.log("onclose: "+e);
    await start();
});

