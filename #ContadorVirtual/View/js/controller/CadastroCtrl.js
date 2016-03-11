    var app = angular.module("cadastroMCV", ["ngMessages"]).config(function ($httpProvider) {
      $httpProvider.defaults.headers.common['Access-Control-Allow-Origin'] = '*';
    });
     angular.module("cadastroMCV").controller("cadastroMCVCtrl", function ($scope, $http) {
      $scope.app = "Meu Contador Virtual";
      $scope.showMe="1";
      $scope.formPessoa = false;
      $scope.passo = true;
      $scope.formEmpresa = true;
        $http.get('http://localhost:61406/api/Users')
            .then(function (response) {
                $scope.usuarios = response.data;
                console.log($scope.usuarios);
            }, function (response) {
                console.log("nao conseguimos dados do seu WS");
            });
      
      $scope.adicionarUsuario = function (usuario) {
        $http({
                url: "http://localhost:61406/api/users",
                method: "POST",
                data: JSON.stringify(usuario),
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8'
                }
            });


        /*usuario.data = new Date();
        $http.post("http://localhost:61406/api/", usuario).success(function(data){
          $scope.showMe=1;*/
        }
        
      
      $scope.Proximo = function(index){
        $scope.showMe = index;
      }
      $scope.adicionarEmpresa = function(empresa)
      {
 
        $scope.formPessoa = true;
        $scope.formEmpresa = false; 
        $http.post("http://localhost:61406/api/Empresas", empresa).success(function(data){
       
            delete $scope.empresa;
            window.location.href ="/pagamento.html"
            $scope.formUser.$setPristine();

        })
      };

      $scope.alterarStatus = function()
      {
 
        $scope.formPessoa = true;
        $scope.formEmpresa = false; 
        $scope.passo = true; 
       
      };
      $scope.apagarContato = function(contatos) {
         $scope.contatos= contatos.filter(function(contato){
          if (!contato.selecionado) return contato; 
        });
      };
      $scope.isContatoSelecionado = function(contatos){
        return contatos.some(function(contato){
          return contato.selecionado;
        });
      };
      $scope.ordenarPor = function(campo){
        $scope.criterioDeOrdenacao = campo;
        $scope.direcaoOrdenacao = !$scope.direcaoOrdenacao;
      }

      
    });
