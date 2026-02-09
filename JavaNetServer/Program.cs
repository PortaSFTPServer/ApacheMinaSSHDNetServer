// See https://aka.ms/new-console-template for more information

using com.sun.crypto.provider;
using java.nio.file;
using java.util;
using org.apache.sshd.common.file.virtualfs;
using org.apache.sshd.server;
using org.apache.sshd.server.keyprovider;
using org.apache.sshd.server.session;
using org.apache.sshd.sftp.server;

namespace JavaNetServer
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // 1. Create the server using default settings
            SshServer sshd = SshServer.setUpDefaultServer();

            // 2. Configure Port
            sshd.setPort(2222);

            // 3. Set Host Key Provider (IKVM maps Paths.get to java.nio.file.Paths)
            var keyPath = Paths.get(".\\hostkey.ser");

            sshd.setKeyPairProvider(new SimpleGeneratorHostKeyProvider(keyPath));


            string physicalPath = ".\\admin"; // Ensure this directory exists

            if (!System.IO.Directory.Exists(physicalPath)) System.IO.Directory.CreateDirectory(physicalPath);

            // 2. Create the Factory and set the default home (root)
            var fileSystemFactory = new VirtualFileSystemFactory(Paths.get(physicalPath));

            // 3. Attach it to the server
            sshd.setFileSystemFactory(fileSystemFactory);


            sshd.setPasswordAuthenticator(new MyCustomAuthenticator(fileSystemFactory));


            // SFTP subsystem with chunked encryption accessor
            SftpSubsystemFactory sftpFactory = new SftpSubsystemFactory.Builder()
                //.withFileSystemAccessor()
                .build();



            // sftpFactory.addSftpEventListener(new MySftpEventListener());

            sshd.setSubsystemFactories(Collections.singletonList(sftpFactory));


            // 5. Start the server
            sshd.start();

            Console.WriteLine("IKVM-hosted SSH Server started on port 2222...");


        }
    }


}

