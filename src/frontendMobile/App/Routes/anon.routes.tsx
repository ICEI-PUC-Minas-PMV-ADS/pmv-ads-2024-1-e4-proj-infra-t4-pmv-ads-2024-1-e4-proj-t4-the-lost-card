import { createNativeStackNavigator } from '@react-navigation/native-stack';
import SplashPage from '../Pages/SplashPage';
import HomePage from '../Pages/HomePage';
import SignIn from '../Pages/SignInPage';

const { Screen, Navigator } = createNativeStackNavigator();

const AnonRoutes = () => {
    return (
        <Navigator screenOptions={{ headerShown: false }} initialRouteName='SplashPage'>
            {/* <Screen name="DefaultAnnon" component={DefaultAnnonPage} />  */}
            <Screen name="SplashPage" component={SplashPage} />
            <Screen name="HomePage" component={HomePage} />
            <Screen name="SignInPage" component={SignIn} />
        </Navigator>
    )
}

export default AnonRoutes;