import { TestBed } from '@angular/core/testing';
import { CookieService } from 'ngx-cookie-service';
import { UserCookieService } from '../../Model/services/UserCookieService';
import { User } from '../../Model/User';

describe('Test du service UserCookieService', () => {
  let service: UserCookieService;
  let cookieServiceMock: jasmine.SpyObj<CookieService>;

  beforeEach(() => {
    // Crée un mock du service CookieService
    cookieServiceMock = jasmine.createSpyObj('CookieService', ['set', 'get', 'delete']);

    TestBed.configureTestingModule({
      providers: [
        UserCookieService,
        { provide: CookieService, useValue: cookieServiceMock }
      ]
    });

    service = TestBed.inject(UserCookieService);
  });

  it('devrait définir et obtenir le token correctement', () => {
    const token = 'test-token'; 
    service.setToken(token);

    // Vérifier que la méthode set de CookieService a été appelée avec la bonne clé et valeur
    expect(cookieServiceMock.set).toHaveBeenCalledWith('authToken', token);

    // Vérifier que la méthode get retourne bien le token
    cookieServiceMock.get.and.returnValue(token); // On simule la valeur retournée par la méthode get
    expect(service.getToken()).toBe(token);

    // Vérifier que le BehaviorSubject émet la bonne valeur
    service.getTokenObservable().subscribe(value => {
      expect(value).toBe(token);
    });
  });

  it('devrait supprimer le token correctement', () => {
    service.deleteToken();

    // Vérifier que la méthode delete de CookieService a été appelée avec la bonne clé
    expect(cookieServiceMock.delete).toHaveBeenCalledWith('authToken');

    // Vérifier que le BehaviorSubject est mis à jour avec une valeur vide
    service.getTokenObservable().subscribe(value => {
      expect(value).toBe('');
    });
  });

  it('devrait définir et obtenir l utilisateur correctement', () => {
    const user = new User('john_doe', 'john@example.com', 1500);
    service.setUser(user);

    // Vérifier que la méthode set de CookieService a été appelée avec la bonne clé et valeur
    const userData = JSON.stringify({ username: user.Username, email: user.Email, elo: user.Elo });
    expect(cookieServiceMock.set).toHaveBeenCalledWith('authUser', userData);

    // Vérifier que la méthode get retourne bien l'utilisateur
    cookieServiceMock.get.and.returnValue(userData); // On simule la valeur retournée par la méthode get
    expect(service.getUser()).toEqual(user);

    // Vérifier que le BehaviorSubject émet la bonne valeur
    service.getUserObservable().subscribe(value => {
      expect(value).toEqual(user);
    });
  });

  it('devrait supprimer l utilisateur correctement', () => {
    service.deleteUser();

    // Vérifier que la méthode delete de CookieService a été appelée avec la bonne clé
    expect(cookieServiceMock.delete).toHaveBeenCalledWith('authUser');

    // Vérifier que le BehaviorSubject est mis à jour avec null
    service.getUserObservable().subscribe(value => {
      expect(value).toBeNull();
    });
  });
});
