using java.nio.file;
using org.apache.sshd.common.file;
using org.apache.sshd.common.file.virtualfs;
using org.apache.sshd.server.auth.password;
using org.apache.sshd.server.session;
using System;
using System.Collections.Generic;
using System.Text;

namespace JavaNetServer
{
    // Implementing the Java interface explicitly
    public class MyCustomAuthenticator : java.lang.Object, PasswordAuthenticator
    {
        private readonly VirtualFileSystemFactory fileSystemFactory;

        public MyCustomAuthenticator(VirtualFileSystemFactory fileSystemFactory)
        {
            this.fileSystemFactory = fileSystemFactory;
        }

        public bool authenticate(string username, string password, ServerSession session)
        {
            bool result = username == "admin" && password == "secret";

            if (result)
            {

                fileSystemFactory.setUserHomeDir("admin", Paths.get(".\\admin"));
            }

            return result;
        }

        bool PasswordAuthenticator.handleClientPasswordChangeRequest(ServerSession session, string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
