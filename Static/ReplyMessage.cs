﻿namespace WebApi_SGI_T.Static
{
    public class ReplyMessage
    {
        public static string MESSAGE_QUERY = "Consulta realizada con exito!";
        public static string MESSAGE_QUERY_EMPTY = "No se encontraron registros!";
        public static string MESSAGE_SAVE = "Registro guardado con exito!";
        public static string MESSAGE_UPDATE = "Registro actualizado con exito!";
        public static string MESSAGE_DELETE = "Registro eliminado con exito!";
        public static string MESSAGE_EXISTS = "El registro ya existe.";
        public static string MESSAGE_ACTIVATE = "Registro activado con exito!";
        public static string MESSAGE_TOKEN = "Token generado con exito!";
        public static string MESSAGE_TOKEN_ERROR = "Nombre de usuario o contraseña no válidos.";
        public static string MESSAGE_VALIDATE = "Errores de validacion.";
        public static string MESSAGE_FAILED = "Ocurrio un error al procesar la solicitud!";
        public static string MESSAGE_ANNULAR = "Constancia anulada correctamente!.";

        //USER ERRORS
        public const string MESSAGE_USERNAME = "El nombre de usuario es oblicatorio!";
        public const string MESSAGE_FAILED_PASS = "La contraseña debe tener al menos 8 caracteres.";
        public const string MESSAGE_FAILED_MIN_PASS = "La contraseña debe tener al menos 8 caracteres.";
    }
}
