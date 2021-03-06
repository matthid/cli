﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.DotNet.Cli.Utils;
using Microsoft.Extensions.EnvironmentAbstractions;

namespace Microsoft.DotNet.Configurer
{
    public class AspNetCertificateSentinel : IAspNetCertificateSentinel
    {
        public static readonly string SENTINEL = $"{Product.Version}.aspNetCertificateSentinel";

        private readonly IFile _file;
        private readonly IDirectory _directory;

        private string _dotnetUserProfileFolderPath;

        private string SentinelPath => Path.Combine(_dotnetUserProfileFolderPath, SENTINEL);

        public AspNetCertificateSentinel(CliFolderPathCalculator cliFallbackFolderPathCalculator) :
            this(
                CliFolderPathCalculator.DotnetUserProfileFolderPath,
                FileSystemWrapper.Default.File,
                FileSystemWrapper.Default.Directory)
        {
        }

        internal AspNetCertificateSentinel(string dotnetUserProfileFolderPath, IFile file, IDirectory directory)
        {
            _file = file;
            _directory = directory;
            _dotnetUserProfileFolderPath = dotnetUserProfileFolderPath;
        }

        public bool Exists()
        {
            return _file.Exists(SentinelPath);
        }

        public void CreateIfNotExists()
        {
            if (!Exists())
            {
                if (!_directory.Exists(_dotnetUserProfileFolderPath))
                {
                    _directory.CreateDirectory(_dotnetUserProfileFolderPath);
                }

                _file.CreateEmptyFile(SentinelPath);
            }
        }

        public void Dispose()
        {
        }
    }
}
