
   alert("teste");

    var app = angular.module("cadastroMCV", ["ngMessages"]);
     angular.module("cadastroMCV").controller("cadastroMCVCtrl", function ($scope, $http) {
      $scope.app = "Meu Contador Virtual";
      $scope.formPessoa = false;
      $scope.passo = true;
      $scope.formEmpresa = true;
      $scope.showMe="1";
      alert("aqui");
      console.log($scope.showMe);
    
      
      $scope.adicionarUsuario = function (usuario) {
        usuario.data = new Date();
        $scope.formPessoa = true;
        $scope.formEmpresa = true; 
        $scope.passo = false; 
        $http.post("http://localhost:61406/api/v1/public/users", usuario).sucess(function(data){
       
            delete $scope.usuario;
            $scope.formUser.$setPristine();

        })
        
      };
      $scope.Proximo = function(index){
        $scope.showMe = index;
      }
      $scope.adicionarEmpresa = function(empresa)
      {
 
        $scope.formPessoa = true;
        $scope.formEmpresa = false; 
        $http.post("http://localhost:61406/api/Empresas", empresa).sucess(function(data){
       
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
