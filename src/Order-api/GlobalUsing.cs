global using System.Text;
global using System.Text.Json;
global using System.Globalization;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Caching.Distributed;
global using RabbitMQ.Client;
global using RabbitMQ.Client.Events;

global using Order_api.Enums;
global using Order_api.Models;
global using Order_api.Services;
global using Order_api.Exceptions;
global using Order_api.Services.Caching;
global using Order_api.RabbitMQ;