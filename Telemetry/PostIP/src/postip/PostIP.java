package postip;

import com.dropbox.core.*;
import java.io.*;
import java.nio.file.*;
import java.util.Locale;

/**
 * Application to post an IP address to a dropbox app.
 */
public class PostIP {

    /**
     * Main function for PostIP application. Posts an IP address
     * to a dropbox app using the given access token.
     * @param args the command line arguments (first is access token, 
     *             second is IP address).
     */
    public static void main(String[] args) {
        try {
            postIP(args[0], args[1]);
        }
        catch (IOException ioe) {
            System.out.print("IOException: ");
            System.out.println(ioe.getMessage());
        }
        catch (DbxException dbe) {
            System.out.print("DbxException: ");
            System.out.println(dbe.getMessage());
        }
    }
    
    /**
     * Posts an IP address to a dropbox app using the given access token.
     * @param accessToken the access token for accessing the dropbox app
     * @param ipAddress the IP address to post
     * @throws IOException
     * @throws DbxException
     */
    public static void postIP(String accessToken, String ipAddress) 
            throws IOException, DbxException  {
        final String FILENAME = "IP.txt";
        
        // write IP address to file
        Path path = Paths.get(FILENAME);
        BufferedWriter writer = Files.newBufferedWriter(path,
                StandardOpenOption.CREATE, StandardOpenOption.WRITE, StandardOpenOption.TRUNCATE_EXISTING);     
        writer.write(ipAddress);
        writer.close();
        
        // post file to dropbox
        DbxRequestConfig config = new DbxRequestConfig("PostIP", Locale.getDefault().toString());
        DbxClient client = new DbxClient(config, accessToken);
        File inputFile = new File(FILENAME);
        FileInputStream inputStream = new FileInputStream(inputFile);
        DbxEntry.File uploadedFile = client.uploadFile("/IP.txt",
            DbxWriteMode.force(), inputFile.length(), inputStream);
        System.out.println("Uploaded: " + uploadedFile.toString());
        inputStream.close();
    }
}
