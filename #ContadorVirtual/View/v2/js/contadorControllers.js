var contadorControllers = angular.module('contadorControllers', []);

contadorControllers.controller('indexCtrl', function ($scope, $http, $location) {
    $scope.meu_nome = "teste";
alert("carregou");
    $http.get(site_config.endereco_ws + "?_action=1&Ofertas=1&_token=" + site_config.token_parceiro)
            .then(function (response) {
                $scope.ofertas = response.data.data;
            }, function (response) {
                console.log("nao conseguimos dados do seu WS");
            });
            
     $http.get(site_config.endereco_ws + "?_action=1&_limit=20&_page=0&_token=" + site_config.token_parceiro)
            .then(function (response) {
                $scope.carros = response.data.data;
        alert(JSON.stringify($scope.carros));
            }, function (response) {
                console.log("nao conseguimos dados do seu WS");
            });        

    $scope.marcas = function () {
        console.log("Buscando marcas");
        if (localStorage.getItem('marcas') === null) {
            console.log("Marcas nao existem no sistema");
            $http.get(site_config.endereco_ws + "?_action=4&_page=0&_limit=4&_token=" + site_config.token_parceiro).then(
                function (response) {
                    $scope.marcas = response.data.data;
                    localStorage.setItem('marcas', JSON.stringify($scope.marcas));
                    localStorage.setItem('marcas_hash', md5(JSON.stringify($scope.marcas)));
                },function (error) {console.log(JSON.stringify(error.data));});
        } else {
            $scope.marcas = JSON.parse(localStorage.getItem('marcas'));
            console.log("Marcas já existem no sistema");
            $http.get(site_config.endereco_ws + "?_action=4&_page=0&_limit=4&_token=" + site_config.token_parceiro).then(
                function (response) {
                    //$scope.marcas = response.data.data;
                    var marcas_antigas = localStorage.getItem('marcas_hash', md5(JSON.stringify($scope.marcas)));
                    var novas_marcas = md5(JSON.stringify(response.data.data));
                    if(marcas_antigas != novas_marcas){
                        console.log("Atualizando marcas do sistema");
                        $scope.marcas = response.data.data;
                        localStorage.setItem('marcas', JSON.stringify($scope.marcas));
                        localStorage.setItem('marcas_hash', md5(JSON.stringify($scope.marcas)));
                    }
                },function (error) {console.log(JSON.stringify(error.data));});
            
            $scope.marcas = JSON.parse(localStorage.getItem('marcas'));
        }
    }

    $scope.marcas();

});

contadorControllers.controller('marcaCtrl', function ($scope, $http, $location, $routeParams) {
    var id = $routeParams.id;
    $http.get(site_config.endereco_ws + "?_action=1&MarcaID=" + id + "&_page=0&_limit=10&_token=" + site_config.token_parceiro)
            .then(function (response) {
                $scope.modelos = response.data.data;
            }, function (response) {
                console.log("nao conseguimos dados do seu WS");
            });

});



contadorControllers.controller('frotaCtrl', function ($scope, $http, $location, $routeParams) {
    var id = $routeParams.id;
    $http.get(site_config.endereco_ws + "?_action=2&FrotaID=" + id + "&_token=" + site_config.token_parceiro)
            .then(function (response) {
                $scope.carro = response.data.data[0];
            }, function (response) {
                console.log("nao conseguimos dados do seu WS");
            });

});


contadorControllers.controller('pageCtrl', function ($scope, $http, $location) {

    $scope.meu_nome = "Daniel";
    $scope.site_nome = site_config.site_nome;



});

var salvar = function (url, scope, http, location) {
    http.post(url, scope.entidade)
            .success(function (data, status, headers, config) {
                scope.successo = data;
                console.log("Sucesso!");
                location.path("/listar-solicitacoes");
            })
            .error(function (data, status, headers, config) {
                scope.error = data;
            });
};

var carregar = function (url, scope, http) {
    http.get(url, scope.entidade)
            .success(function (data, status, headers, config) {
                scope.entidades = data;
            })
            .error(function (data, status, headers, config) {
                scope.error = data;
            });
};


contadorControllers.controller('genericCtrl', function ($scope, $http, $location) {
    $scope.perfil = localStorage.getItem('perfil');


    $scope.salvar_entidade = function (entidade) {
        salvar(appPath + entidade, $scope, $http, $location);
        delete $scope.entidade;
    };
    $scope.carregar_entidades = function (entidade) {
        carregar(appPath + entidade, $scope, $http);
    };

    $scope.aceitar_proposta = function (proposta) {
        proposta.status = "aprovada";
        $scope.entidade = {
            status: 'aprovada'
        };
        $http.put(appPath + 'propostas/' + proposta.id, $scope.entidade)
                .success(function (data, status, headers, config) {
                    console.log(data);
                })
                .error(function (data, status, headers, config) {
                    console.log(data);
                });
    };

    $scope.recusar_proposta = function (proposta) {
        proposta.status = "recusada";
        $scope.entidade = {
            status: 'recusada'
        };
        $http.put(appPath + 'propostas/' + proposta.id, $scope.entidade)
                .success(function (data, status, headers, config) {
                    console.log(data);
                })
                .error(function (data, status, headers, config) {
                    console.log(data);
                });

    };

});


contadorControllers.controller('cadastroCtrl', function ($scope, $http) {
  // alert("teste");
});



contadorControllers.controller('loginCtrl', function ($scope, $http) {
    $scope.logar = function () {
        $http.post(appPath + 'usuarios/login', $scope.entidade)
                .success(function (data, status, headers, config) {
                    $scope.token = data;
                    localStorage.setItem('hash_login', data.token);
                    localStorage.setItem('usuario_logado', JSON.stringify(data));
                    localStorage.setItem('logado', 1);
                    if (data.perfil == "prestador") {
                        localStorage.setItem('perfil', 1);
                        $scope.perfil = 1;
                        window.location.href = '#/listar-solicitacoes';
                    } else {
                        localStorage.setItem('perfil', 2);
                        $scope.perfil = 2;
                        window.location.href = '#/listar-proposta';
                    }
                })
                .error(function (data, status, headers, config) {
                    $scope.error = data;
                });
    }
});