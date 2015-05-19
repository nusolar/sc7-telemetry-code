package accesstoken;

import com.dropbox.core.*;
import java.io.*;
import java.util.Locale;

/**
 * Application to get the access token for a dropbox app. 
 * Copy the app key and app secret from the dropbox app console into the constants
 * declared at the top of the main function.
 */
public class AccessToken {

    /**
     * Main function for AccessToken application.
     * @param args the command line arguments (none)
     */
    public static void main(String[] args) {
        // Get your app key and secret from the Dropbox developers website.
        final String APP_KEY = "yd7c6qd3cl323p8";
        final String APP_SECRET = "ptbp0vjz6721wto";

        try {
            String accessToken = getAccessToken(APP_KEY, APP_SECRET);
            System.out.println(accessToken);
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
     * Gets the access token for the dropbox app corresponding to the given
     * app key and app secret
     * @param appKey the app key
     * @param appSecret the app secret
     * @return the access token
     * @throws IOException
     * @throws DbxException 
     */
    public static String getAccessToken(String appKey, String appSecret)
            throws IOException, DbxException {
        
        DbxAppInfo appInfo = new DbxAppInfo(appKey, appSecret);
        DbxRequestConfig config = new DbxRequestConfig("AccessToken",
            Locale.getDefault().toString());
        DbxWebAuthNoRedirect webAuth = new DbxWebAuthNoRedirect(config, appInfo);

        // Have the user sign in and authorize your app.
        String authorizeUrl = webAuth.start();
        System.out.println("1. Go to: " + authorizeUrl);
        System.out.println("2. Click \"Allow\" (you might have to log in first)");
        System.out.println("3. Copy the authorization code.");
        String code = new BufferedReader(new InputStreamReader(System.in)).readLine().trim();

        // This will fail if the user enters an invalid authorization code.
        DbxAuthFinish authFinish = webAuth.finish(code);
        String accessToken = authFinish.accessToken;
        
        return accessToken;
    }
}
