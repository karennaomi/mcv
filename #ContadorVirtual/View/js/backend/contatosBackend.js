var express = require('express');
var app = express();
var bodyParser = require('body-parser');

app.use(express.static(__dirname + '/public'));

    // parse application/json
    app.use(bodyParser.json());                        

    // parse application/x-www-form-urlencoded
    app.use(bodyParser.urlencoded({ extended: true }));


var contatos = [
	{nome: "Bruno", telefone: "9999-2222", data: new Date(), operadora: {nome: "Oi", codigo: 14, categoria: "Celular"}},
	{nome: "Sandra", telefone: "9999-3333", data: new Date(), operadora: {nome: "Vivo", codigo: 15, categoria: "Celular"}},
	{nome: "Mariana", telefone: "9999-9999", data: new Date(), operadora: {nome: "Tim", codigo: 41, categoria: "Celular"}}
];
var operadoras = [
	{nome: "Oi", codigo: 14, categoria: "Celular", preco: 2},
	{nome: "Vivo", codigo: 15, categoria: "Celular", preco: 1},
	{nome: "Tim", codigo: 41, categoria: "Celular", preco: 3},
	{nome: "GVT", codigo: 25, categoria: "Fixo", preco: 1},
	{nome: "Embratel", codigo: 21, categoria: "Fixo", preco: 2}
];

app.listen(process.env.PORT || 61400);

app.all('*', function(req, res, next) {
  res.header('Access-Control-Allow-Origin', '*');
  res.header('Access-Control-Allow-Methods', 'PUT, GET, POST, DELETE, OPTIONS');
  res.header('Access-Control-Allow-Headers', 'Content-Type');
  next();
});


