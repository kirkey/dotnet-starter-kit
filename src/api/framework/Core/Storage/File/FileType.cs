using System.ComponentModel;

namespace FSH.Framework.Core.Storage.File;

public enum FileType
{
    [Description(".jpg,.png,.jpeg")] Image,
    [Description(".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.txt,.rtf,.odt,.csv")] Document,
    [Description(".zip,.rar,.7z,.tar,.gz,.tgz,.bz2,.xz")] ZipFile
}
