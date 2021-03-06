(function () {

    var token = Cookies.get("TokenNotCo");
    // Autenticación custom para guardar token
    tableau.password = token;
    console.log(tableau.password);

    // Create the connector object
    var myConnector = tableau.makeConnector();
    var reintentos = 0;
    var limiteReintentos = 30;

    // Manejo de errores
    var reintentos = 0;
    var limiteReintentos = 30;

    $.ajaxSetup({
        error: function (jqXHR, textStatus, errorThrown) {
            if (jqXHR.status === 0) {
                console.log('Not connect: Verify Network.');
                if (reintentos >= limiteReintentos)
                    tableau.abortWithError('*** Not connect: Verify Network. ***');
            } else if (jqXHR.status == 401) {
                tableau.abortWithError('*** Token doesn\'t have permision [401] ***');
                console.log('Token doesn\'t have permision [401]');
            } else if (jqXHR.status == 404) {
                tableau.abortWithError('*** Requested page not found [404] ***');
                console.log('Requested page not found [404]');
            } else if (jqXHR.status == 429) {
                console.log('Too many request (Retries: ' + reintentos + ')[429]');
                if (reintentos >= limiteReintentos)
                    tableau.abortWithError('*** Too many request (Retries: ' + reintentos + ') [429] ***');
            } else if (jqXHR.status == 500) {
                tableau.abortWithError('*** Internal Server Error [500] ***');
                console.log('Internal Server Error [500].');
            } else if (textStatus === 'parsererror') {
                tableau.abortWithError('*** Requested JSON parse failed. ***');
                console.log('Requested JSON parse failed.');
            } else if (textStatus === 'timeout') {
                tableau.abortWithError('*** Time out error. ***');
                console.log('Time out error.');
            } else if (textStatus === 'abort') {
                tableau.abortWithError('*** Ajax request aborted. ***');
                console.log('Ajax request aborted.');
            } else {
                tableau.abortWithError('*** Uncaught Error: ' + jqXHR.responseText + '***');
                console.log('Uncaught Error: ' + jqXHR.responseText);
            }
        }
    });
    myConnector.init = function (initCallback) {
        initCallback();
    }
    // Define the schema
    myConnector.getSchema = function (schemaCallback) {
        var cols = [{
            id: "fecha",
            dataType: tableau.dataTypeEnum.string
        }, {
            id: "sku_b2b",
            dataType: tableau.dataTypeEnum.int
        }, {
            id: "local_b2b",
            dataType: tableau.dataTypeEnum.string
            },
        //    {
        //    id: "holding",
        //    dataType: tableau.dataTypeEnum.string
        //},
        {
            id: "ventas",
            dataType: tableau.dataTypeEnum.float
        },
        {
            id: "unidades",
            dataType: tableau.dataTypeEnum.float
        },
        {
            id: "costo",
            dataType: tableau.dataTypeEnum.float
        }];

        var tableSchema = {
            id: "sales",
            alias: "Respuesta correcta Sales NotCo",
            columns: cols
        };

        schemaCallback([tableSchema]);
    };

    myConnector.getData = function (table, doneCallback) {
        console.log("se hizo click")
        var usuario = $("#usuario").val();
        var password = $("#password").val();
        console.log("Usuario: " + usuario);
        console.log("Password: " + password);

        //Crear Fechas en C#
        //var tableData = [];
        //var AnoSeleccionado = $("#anoSeleccionado").val();
        var AnoSeleccionado = "";
        //Get año seleccionado

        $.ajax({
            url: '../Home/GetAnio/',
            async: false,
            data: { AnoSeleccionado: AnoSeleccionado },
            type: "POST",
            success: function (resp) {
                if (resp != "") {
                    console.log("Año GET por Ajax Global: " + resp);
                    AnoSeleccionado = resp;
                }
            },
            error: function (err) {
                console.log(err);
            }
        });

        
        console.log("Año seleccionado!!: " + AnoSeleccionado);
        var primerDia = "";
        var ultimoDia = "";
        $.ajax({
            url: '../Home/ControlFechasPorAno/',
            async: false,
            data: { AnoSeleccionado: AnoSeleccionado },
            type: "POST",
            success: function (resp) {
                if (resp != "") {
                    var fecha = resp.split(';');
                    primerDia = fecha[0];
                    ultimoDia = fecha[1];
                }
            },
            error: function (err) {
                console.log(err);
            }
        });

        console.log("Fecha actual: " + primerDia);
        console.log("Fecha 1 años despues: " + ultimoDia);

        //Convertir a formato JavaScript
        var dateMomentObject = moment(ultimoDia, "DD/MM/YYYY");
        var Fecha12 = dateMomentObject.toDate();
        var dateMomentObject = moment(primerDia, "DD/MM/YYYY");
        var Fecha1 = dateMomentObject.toDate();

        console.log("Fecha primer dia: " + Fecha1);
        console.log("Fecha 1 años despues: " + Fecha12);

        while (Fecha1 <= Fecha12) {

            var fechaanterior = primerDia;
            //Sumar 7 días en C#
            $.ajax({
                url: '../Home/Sumar7Dias2/',
                async: false,
                data: { primerDia: primerDia },
                type: "POST",
                success: function (resp) {
                    if (resp != "") {
                        primerDia = resp;
                    }
                },
                error: function (err) {
                    console.log(err);
                }
            });
            var fechacomodin = primerDia;
            console.log("Pasaron 7 días: " + primerDia);
            //Convertir a formato JavaScript
            dateMomentObject = moment(primerDia, "DD/MM/YYYY");
            Fecha1 = dateMomentObject.toDate();


            //Cabmiar a formato adecuado para consulta
            $.ajax({
                url: '../Home/CambiarFormato/',
                async: false,
                data: { fechaanterior: fechaanterior, fechacomodin: fechacomodin },
                type: "POST",
                success: function (resp) {
                    if (resp != "") {
                        var fecha = resp.split(';');
                        fechaanterior = fecha[0];
                        fechacomodin = fecha[1];
                    }
                },
                error: function (err) {
                    console.log(err);
                }
            });

            console.log("Se está ejecutando la consulta entre los días: " + fechaanterior + " Y " + fechacomodin);
            $.ajax({
                url: '../Home/ConsultaApiSalesPorAnio/',
                async: false,
                data: { usuario: usuario, password: password, fechaanterior: fechaanterior, fechacomodin: fechacomodin, AnoSeleccionado: AnoSeleccionado },
                type: "POST",
                success: function (resp) {
                    // data is your result from controller
                    var tableData = [];
                    if (resp != "Error: 400 - Bad Request") {
                        const respuesta = JSON.parse(resp);
                        console.log("RESPUESTA");
                        console.log(respuesta);
                        var feat = respuesta;
                        //console.log("FEAT");
                        //console.log(feat);
                        for (var i = 0, len = feat.length; i < len; i++) {

                            // Registro base|
                            tableData.push({
                                "fecha": feat[i].fecha,
                                "sku_b2b": feat[i].sku_b2b,
                                "local_b2b": feat[i].local_b2b,
                                "ventas": feat[i].ventas,
                                "unidades": feat[i].unidades,
                                "costo": feat[i].costo
                            });
                            //tableData.push(venta);


                        }
                        console.log("--------------")
                        table.appendRows(tableData);
                        //tableData.Clear;
                        console.log("--------------")
                        console.log("Número de elementos:" + tableData.length);
                    }
                    else {
                        console.log("RESPUESTA ERROR");
                        console.log(resp);
                    }


                },
                error: function (xhr, status, error) {
                    console.log("ERROR" + err);
                    console.log("Error!" + xhr.status);
                }
            })
            //    .fail(function (jqXHR, textStatus, errorThrown) {
            //    if (jqXHR.status == 429 || jqXHR.status == 503 || jqXHR.status == 0) {
            //        reintentos++;
            //        wait(60000);  // 10 seconds in milliseconds
            //        if (reintentos >= limiteReintentos)
            //            console.log("ERROR 1");
            //    }
            //    else {
            //        ultimaVueltaSucursal = true;
            //        console.log("ERROR 2");
            //    }
            //});
            //Sumar 1 día para la siguiente consulta
            $.ajax({
                url: '../Home/SumarUnDia/',
                async: false,
                data: { fechacomodin: fechacomodin },
                type: "POST",
                success: function (resp) {
                    if (resp != "") {
                        primerDia = resp;
                    }
                },
                error: function (err) {
                    console.log(err);
                }
            });
        }
        //table.appendRows(tableData);
        doneCallback();
        console.log("terminó!")
        console.log(table)
        console.log("Número de elementos:" + table.length);
        
    };

    tableau.registerConnector(myConnector);
    if (!!window.tableauVersionBootstrap) {
        window._tableau.triggerInitialization();
    }
    if (!window.tableauVersionBootstrap) {
        var DOMContentLoaded_event = window.document.createEvent("Event");
        DOMContentLoaded_event.initEvent("DOMContentLoaded", true, true);
        window.document.dispatchEvent(DOMContentLoaded_event);
    }

})();