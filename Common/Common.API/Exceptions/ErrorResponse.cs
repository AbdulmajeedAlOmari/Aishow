using System.Net;
using Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Common.Exceptions;

    public class ErrorResponse
    {
        private readonly string _messageAr;
        private readonly string _messageEn;
        private readonly string _detailAr;
        private readonly string _detailEn;
        private readonly string _type;
        private readonly HttpStatusCode _httpStatusCode;

        public string ErrorLog
        {
            get
            {
                return $"ApiException: status {_httpStatusCode} and message {_messageEn}";
            }
        }

        public ErrorResponse(HttpStatusCode httpStatusCode, string messageEn, string messageAr, string type = "about:blank")
        {
            _messageAr = messageAr;
            _messageEn = messageEn;
            _type = type;
            _httpStatusCode = httpStatusCode;
        }

        public ErrorResponse(HttpStatusCode httpStatusCode, string messageEn, string messageAr, string detailEn, string detailAr, string type = "about:blank")
        {
            _messageAr = messageAr;
            _messageEn = messageEn;
            _detailAr = detailAr;
            _detailEn = detailEn;
            _type = type;
            _httpStatusCode = httpStatusCode;
        }

        private string GetErrorMessageByLang(LanguageEnum language)
        {
            if (language == LanguageEnum.English)
                return _messageEn;

            return _messageAr;
        }

        private string GetDetailByLang(LanguageEnum language)
        {
            if (string.IsNullOrEmpty(_detailAr) && string.IsNullOrEmpty(_detailEn))
            {
                return GetErrorMessageByLang(language);
            }

            if (language == LanguageEnum.English)
                return _detailEn;

            return _detailAr;
        }

        public ProblemDetails GetProblemDetailsByLang(LanguageEnum language)
        {
            return new ProblemDetails
            {
                Title = GetErrorMessageByLang(language),
                Detail = GetDetailByLang(language),
                Status = (int)_httpStatusCode,
                Type = _type,
            };
        }

    }

